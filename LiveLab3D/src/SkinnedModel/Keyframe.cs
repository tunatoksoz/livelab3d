#region File Description

//-----------------------------------------------------------------------------
// Keyframe.cs
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
	using Microsoft.Xna.Framework;

	/// <summary>
	/// Describes the position of a single bone at a single point in time.
	/// </summary>
	public class Keyframe
	{
		#region Fields

		private readonly int boneValue;
		private readonly TimeSpan timeValue;
		private readonly Matrix transformValue;

		#endregion

		/// <summary>
		/// Constructs a new keyframe object.
		/// </summary>
		public Keyframe(int bone, TimeSpan time, Matrix transform)
		{
			this.boneValue = bone;
			this.timeValue = time;
			this.transformValue = transform;
		}


		/// <summary>
		/// Gets the index of the target bone that is animated by this keyframe.
		/// </summary>
		public int Bone
		{
			get { return this.boneValue; }
		}


		/// <summary>
		/// Gets the time offset from the start of the animation to this keyframe.
		/// </summary>
		public TimeSpan Time
		{
			get { return this.timeValue; }
		}


		/// <summary>
		/// Gets the bone transform for this keyframe.
		/// </summary>
		public Matrix Transform
		{
			get { return this.transformValue; }
		}
	}
}