#region File Description

//-----------------------------------------------------------------------------
// TypeReaders.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements

#endregion

namespace SkinnedModel
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;

	/// <summary>
	/// Loads SkinningData objects from compiled XNB format.
	/// </summary>
	public class SkinningDataReader : ContentTypeReader<SkinningData>
	{
		protected override SkinningData Read(ContentReader input,
		                                     SkinningData existingInstance)
		{
			IDictionary<string, AnimationClip> animationClips;
			IList<Matrix> bindPose, inverseBindPose;
			IList<int> skeletonHierarchy;
			IDictionary<string, int> boneMap;

			animationClips = input.ReadObject<IDictionary<string, AnimationClip>>();
			bindPose = input.ReadObject<IList<Matrix>>();
			inverseBindPose = input.ReadObject<IList<Matrix>>();
			skeletonHierarchy = input.ReadObject<IList<int>>();
			boneMap = input.ReadObject<IDictionary<string, int>>();

			return new SkinningData(animationClips, bindPose,
			                        inverseBindPose, skeletonHierarchy,
			                        boneMap);
		}
	}


	/// <summary>
	/// Loads AnimationClip objects from compiled XNB format.
	/// </summary>
	public class AnimationClipReader : ContentTypeReader<AnimationClip>
	{
		protected override AnimationClip Read(ContentReader input,
		                                      AnimationClip existingInstance)
		{
			var duration = input.ReadObject<TimeSpan>();
			var keyframes = input.ReadObject<IList<Keyframe>>();

			return new AnimationClip(duration, keyframes);
		}
	}


	/// <summary>
	/// Loads Keyframe objects from compiled XNB format.
	/// </summary>
	public class KeyframeReader : ContentTypeReader<Keyframe>
	{
		protected override Keyframe Read(ContentReader input,
		                                 Keyframe existingInstance)
		{
			var bone = input.ReadObject<int>();
			var time = input.ReadObject<TimeSpan>();
			var transform = input.ReadObject<Matrix>();

			return new Keyframe(bone, time, transform);
		}
	}
}