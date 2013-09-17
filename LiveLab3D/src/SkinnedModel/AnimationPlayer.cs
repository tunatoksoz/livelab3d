#region File Description

//-----------------------------------------------------------------------------
// AnimationPlayer.cs
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

	/// <summary>
	/// The animation player is in charge of decoding bone position
	/// matrices from an animation clip.
	/// </summary>
	public class AnimationPlayer
	{
		#region Fields

		// Information about the currently playing animation clip.
		private readonly Matrix[] boneTransforms;


		// Current animation transform matrices.
		private readonly Matrix[] skinTransforms;


		// Backlink to the bind pose and skeleton hierarchy data.
		private readonly SkinningData skinningDataValue;
		private readonly Matrix[] worldTransforms;
		private AnimationClip currentClipValue;
		private int currentKeyframe;
		private TimeSpan currentTimeValue;

		#endregion

		private List<BoneManipulator> boneManipulators = new List<BoneManipulator>();


		/// <summary>
		/// Constructs a new animation player.
		/// </summary>
		public AnimationPlayer(SkinningData skinningData)
		{
			if (skinningData == null)
				throw new ArgumentNullException("skinningData");

			this.skinningDataValue = skinningData;

			this.boneTransforms = new Matrix[skinningData.BindPose.Count];
			this.worldTransforms = new Matrix[skinningData.BindPose.Count];
			this.skinTransforms = new Matrix[skinningData.BindPose.Count];
		}

		public List<BoneManipulator> BoneManipulators
		{
			get { return this.boneManipulators; }
			set { this.boneManipulators = value; }
		}

		/// <summary>
		/// Gets the clip currently being decoded.
		/// </summary>
		public AnimationClip CurrentClip
		{
			get { return this.currentClipValue; }
		}


		/// <summary>
		/// Gets the current play position.
		/// </summary>
		public TimeSpan CurrentTime
		{
			get { return this.currentTimeValue; }
		}


		/// <summary>
		/// Starts decoding the specified animation clip.
		/// </summary>
		public void StartClip(AnimationClip clip)
		{
			if (clip == null)
				throw new ArgumentNullException("clip");

			this.currentClipValue = clip;
			this.currentTimeValue = TimeSpan.Zero;
			this.currentKeyframe = 0;

			// Initialize bone transforms to the bind pose.
			this.skinningDataValue.BindPose.CopyTo(this.boneTransforms, 0);
		}


		/// <summary>
		/// Advances the current animation position.
		/// </summary>
		public void Update(TimeSpan time, bool relativeToCurrentTime,
		                   Matrix rootTransform)
		{
			UpdateBoneTransforms(time, relativeToCurrentTime);
			UpdateWorldTransforms(rootTransform);
			UpdateSkinTransforms();
		}


		/// <summary>
		/// Helper used by the Update method to refresh the BoneTransforms data.
		/// </summary>
		public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
		{
			if (this.currentClipValue == null)
				throw new InvalidOperationException(
					"AnimationPlayer.Update was called before StartClip");

			// Update the animation position.
			if (relativeToCurrentTime)
			{
				time += this.currentTimeValue;

				// If we reached the end, loop back to the start.
				while (time >= this.currentClipValue.Duration)
					time -= this.currentClipValue.Duration;
			}

			if ((time < TimeSpan.Zero) || (time >= this.currentClipValue.Duration))
				throw new ArgumentOutOfRangeException("time");

			// If the position moved backwards, reset the keyframe index.
			if (time < this.currentTimeValue)
			{
				this.currentKeyframe = 0;
				this.skinningDataValue.BindPose.CopyTo(this.boneTransforms, 0);
			}

			this.currentTimeValue = time;

			// Read keyframe matrices.
			IList<Keyframe> keyframes = this.currentClipValue.Keyframes;

			while (this.currentKeyframe < keyframes.Count)
			{
				Keyframe keyframe = keyframes[this.currentKeyframe];

				// Stop when we've read up to the current time position.
				if (keyframe.Time > this.currentTimeValue)
					break;

				Matrix manipTransform = Matrix.Identity;
				foreach (BoneManipulator boneManip in this.boneManipulators)
				{
					if (this.skinningDataValue.BoneMap[boneManip.BoneName]
					    == keyframe.Bone)
					{
						manipTransform = boneManip.Transform;
					}
				}

				this.boneTransforms[keyframe.Bone] = keyframe.Transform*manipTransform;

				this.currentKeyframe++;
			}

			this.boneManipulators.Clear();
		}


		/// <summary>
		/// Helper used by the Update method to refresh the WorldTransforms data.
		/// </summary>
		public void UpdateWorldTransforms(Matrix rootTransform)
		{
			// Root bone.
			this.worldTransforms[0] = this.boneTransforms[0]*rootTransform;

			// Child bones.
			for (int bone = 1; bone < this.worldTransforms.Length; bone++)
			{
				int parentBone = this.skinningDataValue.SkeletonHierarchy[bone];

				this.worldTransforms[bone] = this.boneTransforms[bone]*
				                             this.worldTransforms[parentBone];
			}
		}


		/// <summary>
		/// Helper used by the Update method to refresh the SkinTransforms data.
		/// </summary>
		public void UpdateSkinTransforms()
		{
			for (int bone = 0; bone < this.skinTransforms.Length; bone++)
			{
				this.skinTransforms[bone] = this.skinningDataValue.InverseBindPose[bone]*
				                            this.worldTransforms[bone];
			}
		}


		/// <summary>
		/// Gets the current bone transform matrices, relative to their parent bones.
		/// </summary>
		public Matrix[] GetBoneTransforms()
		{
			return this.boneTransforms;
		}


		/// <summary>
		/// Gets the current bone transform matrices, in absolute format.
		/// </summary>
		public Matrix[] GetWorldTransforms()
		{
			return this.worldTransforms;
		}


		/// <summary>
		/// Gets the current bone transform matrices,
		/// relative to the skinning bind pose.
		/// </summary>
		public Matrix[] GetSkinTransforms()
		{
			return this.skinTransforms;
		}

		#region Nested type: BoneManipulator

		public struct BoneManipulator
		{
			public string BoneName;
			public Matrix Transform;

			public BoneManipulator(string name, Matrix transform)
			{
				this.BoneName = name;
				this.Transform = transform;
			}
		}

		#endregion
	}
}