using System;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimationScript : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip animationClip;

    private NativeArray<TransformStreamHandle> _handles;
    private NativeArray<float> _boneWeights;
    private PlayableGraph _playableGraph;
    private AnimationScriptPlayable _animationScriptPlayable;

    private void OnEnable()
    {
        var bones = skinnedMeshRenderer.bones;
        var boneLength = bones.Length;

        _handles = new NativeArray<TransformStreamHandle>
            (boneLength, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        _boneWeights = new NativeArray<float>(Enumerable.Repeat(1f, boneLength).ToArray(), Allocator.Persistent);
        for (var i = 0; i < boneLength; i++)
        {
            _handles[i] = animator.BindStreamTransform(bones[i]);
        }

        _playableGraph = PlayableGraph.Create(nameof(AnimationScript));
        _playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        var animationJob = new SingleJob { handles = _handles, boneWeights = _boneWeights };
        _animationScriptPlayable = AnimationScriptPlayable.Create(_playableGraph, animationJob);
        _animationScriptPlayable.AddInput(AnimationClipPlayable.Create(_playableGraph, animationClip), 0, 1f);

        var output = AnimationPlayableOutput.Create(_playableGraph, "Output", animator);
        output.SetSourcePlayable(_animationScriptPlayable);

        _playableGraph.Play();
    }

    private void OnDisable()
    {
        _playableGraph.Destroy();
    }
}