#region File Description

//-----------------------------------------------------------------------------
// SkinningData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements

#endregion

namespace SkinnedModel
{
	using System.Collections.Generic;
	using Microsoft.Xna.Framework;

	/// <summary>
	/// Combines all the data needed to render and animate a skinned object.
	/// This is typically stored in the Tag property of the Model being animated.
	/// </summary>
	public class SkinningData
	{
		#region Fields

		private readonly IDictionary<string, AnimationClip> animationClipsValue;
		private readonly IList<Matrix> bindPoseValue;
		private readonly IList<Matrix> inverseBindPoseValue;
		private readonly IList<int> skeletonHierarchyValue;

		#endregion

		private readonly IDictionary<string, int> boneMap;

		/// <summary>
		/// Constructs a new skinning data object.
		/// </summary>
		public SkinningData(IDictionary<string, AnimationClip> animationClips,
		                    IList<Matrix> bindPose, IList<Matrix> inverseBindPose,
		                    IList<int> skeletonHierarchy,
		                    IDictionary<string, int> boneMap)
		{
			this.boneMap = boneMap;
			this.animationClipsValue = animationClips;
			this.bindPoseValue = bindPose;
			this.inverseBindPoseValue = inverseBindPose;
			this.skeletonHierarchyValue = skeletonHierarchy;
		}

		public IDictionary<string, int> BoneMap
		{
			get { return this.boneMap; }
		}


		/// <summary>
		/// Gets a collection of animation clips. These are stored by name in a
		/// dictionary, so there could for instance be clips for "Walk", "Run",
		/// "JumpReallyHigh", etc.
		/// </summary>
		public IDictionary<string, AnimationClip> AnimationClips
		{
			get { return this.animationClipsValue; }
		}


		/// <summary>
		/// Bindpose matrices for each bone in the skeleton,
		/// relative to the parent bone.
		/// </summary>
		public IList<Matrix> BindPose
		{
			get { return this.bindPoseValue; }
		}


		/// <summary>
		/// Vertex to bonespace transforms for each bone in the skeleton.
		/// </summary>
		public IList<Matrix> InverseBindPose
		{
			get { return this.inverseBindPoseValue; }
		}


		/// <summary>
		/// For each bone in the skeleton, stores the index of the parent bone.
		/// </summary>
		public IList<int> SkeletonHierarchy
		{
			get { return this.skeletonHierarchyValue; }
		}
	}
}