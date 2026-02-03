using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using static CustomDampedTransformJob;

public class CustomDampTransform : RigConstraint<
        CustomDampedTransformJob,
        CustomDampedTransformData,
        CustomDampedTransformJobBinder<CustomDampedTransformData>>
{
    protected override void OnValidate()
    {
        m_Data.dampRotation = Mathf.Clamp01(m_Data.dampRotation);
    }
}

[System.Serializable]
public struct CustomDampedTransformData : IAnimationJobData, ICustomDampedTransformData
{
    [SerializeField] Transform m_ConstrainedObject;

    [SyncSceneToStream, SerializeField] Transform m_Source;
    [SyncSceneToStream, SerializeField, Range(0f, 1f)] float m_DampPosition;
    [SyncSceneToStream, SerializeField, Range(0f, 1f)] float m_DampRotation;

    [NotKeyable, SerializeField] bool m_MaintainAim;

    /// <inheritdoc />
    public Transform constrainedObject { get => m_ConstrainedObject; set => m_ConstrainedObject = value; }
    /// <inheritdoc />
    public Transform sourceObject { get => m_Source; set => m_Source = value; }
    /// <summary>
    /// Damp position weight. Defines how much of constrained object position follows source object position.
    /// Constrained position will closely follow source object when set to 0, and will
    /// not move when set to 1.
    /// </summary>
    public float dampPosition { get => m_DampPosition; set => m_DampPosition = Mathf.Clamp01(value); }
    /// <summary>
    /// Damp rotation weight. Defines how much of constrained object rotation follows source object rotation.
    /// Constrained rotation will closely follow source object when set to 0, and will
    /// not move when set to 1.
    /// </summary>
    public float dampRotation { get => m_DampRotation; set => m_DampRotation = Mathf.Clamp01(value); }
    /// <inheritdoc />
    public bool maintainAim { get => m_MaintainAim; set => m_MaintainAim = value; }

    /// <inheritdoc />
    string ICustomDampedTransformData.dampPositionFloatProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_DampPosition));
    /// <inheritdoc />
    string ICustomDampedTransformData.dampRotationFloatProperty => ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(m_DampRotation));

    /// <inheritdoc />
    bool IAnimationJobData.IsValid() => !(m_ConstrainedObject == null || m_Source == null);

    /// <inheritdoc />
    void IAnimationJobData.SetDefaultValues()
    {
        m_ConstrainedObject = null;
        m_Source = null;
        m_DampPosition = 0.5f;
        m_DampRotation = 0.5f;
        m_MaintainAim = true;
    }
}
[Unity.Burst.BurstCompile]
public struct CustomDampedTransformJob : IWeightedAnimationJob
{
    const float k_FixedDt = 0.01667f; // 60Hz simulation step
    const float k_DampFactor = 40f;

    /// <summary>The Transform handle for the constrained object Transform.</summary>
    public ReadWriteTransformHandle driven;
    /// <summary>The Transform handle for the source object Transform.</summary>
    public ReadOnlyTransformHandle source;

    /// <summary>Initial TR offset from source to constrained object.</summary>
    public AffineTransform localBindTx;

    /// <summary>Aim axis used to adjust constrained object rotation if maintainAim is enabled.</summary>
    public Vector3 aimBindAxis;

    /// <summary>Previous frame driven Transform TR components.</summary>
    public AffineTransform prevDrivenTx;

    /// <summary>
    /// Defines how much of constrained object position follows source object position.
    /// constrained position will closely follow source object when set to 0, and will
    /// not move when set to 1.
    /// </summary>
    public FloatProperty dampPosition;
    /// <summary>
    /// Defines how much of constrained object rotation follows source object rotation.
    /// constrained rotation will closely follow source object when set to 0, and will
    /// not move when set to 1.
    /// </summary>
    public FloatProperty dampRotation;

    /// <inheritdoc />
    public FloatProperty jobWeight { get; set; }

    /// <summary>
    /// Defines what to do when processing the root motion.
    /// </summary>
    /// <param name="stream">The animation stream to work on.</param>
    public void ProcessRootMotion(AnimationStream stream) { }

    /// <summary>
    /// Defines what to do when processing the animation.
    /// </summary>
    /// <param name="stream">The animation stream to work on.</param>
    public void ProcessAnimation(AnimationStream stream)
    {
        float w = jobWeight.Get(stream);
        float streamDt = Mathf.Abs(stream.deltaTime);
        driven.GetGlobalTR(stream, out Vector3 drivenPos, out Quaternion drivenRot);

        if (w > 0f && streamDt > 0f)
        {
            source.GetGlobalTR(stream, out Vector3 sourcePos, out Quaternion sourceRot);
            var sourceTx = new AffineTransform(sourcePos, sourceRot);
            var targetTx = sourceTx * localBindTx;
            targetTx.translation = Vector3.Lerp(drivenPos, targetTx.translation, w);
            targetTx.rotation = Quaternion.Lerp(drivenRot, targetTx.rotation, w);

            var dampPosW = AnimationRuntimeUtils.Square(1f - dampPosition.Get(stream));
            var dampRotW = AnimationRuntimeUtils.Square(1f - dampRotation.Get(stream));
            bool doAimAjustements = Vector3.Dot(aimBindAxis, aimBindAxis) > 0f;

            while (streamDt > 0f)
            {
                float factoredDt = k_DampFactor * Mathf.Min(k_FixedDt, streamDt);

                prevDrivenTx.translation +=
                    (targetTx.translation - prevDrivenTx.translation) * dampPosW * factoredDt;

                prevDrivenTx.rotation *= Quaternion.Lerp(
                    Quaternion.identity,
                    Quaternion.Inverse(prevDrivenTx.rotation) * targetTx.rotation,
                    dampRotW * factoredDt
                    );

                if (doAimAjustements)
                {
                    var fromDir = prevDrivenTx.rotation * aimBindAxis;
                    var toDir = sourceTx.translation - prevDrivenTx.translation;
                    prevDrivenTx.rotation =
                        Quaternion.AngleAxis(Vector3.Angle(fromDir, toDir), Vector3.Cross(fromDir, toDir).normalized) * prevDrivenTx.rotation;
                }

                streamDt -= k_FixedDt;
            }

            driven.SetGlobalTR(stream, prevDrivenTx.translation, prevDrivenTx.rotation);
        }
        else
        {
            prevDrivenTx.Set(drivenPos, drivenRot);
            AnimationRuntimeUtils.PassThrough(stream, driven);
        }
    }
    public interface ICustomDampedTransformData
    {
        /// <summary>The Transform affected by the constraint Source Transform.</summary>
        Transform constrainedObject { get; }
        /// <summary>The source Transform.</summary>
        Transform sourceObject { get; }

        /// <summary>Toggles whether damping will enforces aim.</summary>
        bool maintainAim { get; }

        /// <summary>The path to the damp position weight property in the constraint component.</summary>
        string dampPositionFloatProperty { get; }
        /// <summary>The path to the damp rotation weight property in the constraint component.</summary>
        string dampRotationFloatProperty { get; }
    }
    public class CustomDampedTransformJobBinder<T> : AnimationJobBinder<CustomDampedTransformJob, T>
        where T : struct, IAnimationJobData, ICustomDampedTransformData
    {
        /// <inheritdoc />
        public override CustomDampedTransformJob Create(Animator animator, ref T data, Component component)
        {
            var job = new CustomDampedTransformJob();

            job.driven = ReadWriteTransformHandle.Bind(animator, data.constrainedObject);
            job.source = ReadOnlyTransformHandle.Bind(animator, data.sourceObject);

            var drivenTx = new AffineTransform(data.constrainedObject.position, data.constrainedObject.rotation);
            var sourceTx = new AffineTransform(data.sourceObject.position, data.sourceObject.rotation);

            job.localBindTx = sourceTx.InverseMul(drivenTx);
            job.prevDrivenTx = drivenTx;

            job.dampPosition = FloatProperty.Bind(animator, component, data.dampPositionFloatProperty);
            job.dampRotation = FloatProperty.Bind(animator, component, data.dampRotationFloatProperty);

            if (data.maintainAim && AnimationRuntimeUtils.SqrDistance(data.constrainedObject.position, data.sourceObject.position) > 0f)
                job.aimBindAxis = Quaternion.Inverse(data.constrainedObject.rotation) * (sourceTx.translation - drivenTx.translation).normalized;
            else
                job.aimBindAxis = Vector3.zero;

            return job;
        }

        /// <inheritdoc />
        public override void Destroy(CustomDampedTransformJob job)
        {
        }
    }
}