#region File Description

//-----------------------------------------------------------------------------
// AnimationClip.cs
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

	/// <summary>
	/// An animation clip is the runtime equivalent of the
	/// Microsoft.Xna.Framework.Content.Pipeline.Graphics.AnimationContent type.
	/// It holds all the keyframes needed to describe a single animation.
	/// </summary>
	public class AnimationClip
	{
		private readonly TimeSpan durationValue;
		private readonly IList<Keyframe> keyframesValue;

		/// <summary>
		/// Constructs a new animation clip object.
		/// </summary>
		public AnimationClip(TimeSpan duration, IList<Keyframe> keyframes)
		{
			this.durationValue = duration;
			this.keyframesValue = keyframes;
		}


		/// <summary>
		/// Gets the total length of the animation.
		/// </summary>
		public TimeSpan Duration
		{
			get { return this.durationValue; }
		}


		/// <summary>
		/// Gets a combined list containing all the keyframes for all bones,
		/// sorted by time.
		/// </summary>
		public IList<Keyframe> Keyframes
		{
			get { return this.keyframesValue; }
		}
	}
}