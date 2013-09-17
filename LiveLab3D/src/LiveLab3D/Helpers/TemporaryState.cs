namespace LiveLab3D.Helpers
{
	using System;
	using Microsoft.Xna.Framework.Graphics;

	public class TemporaryState : IDisposable
	{
		private readonly TextureAddressMode addressU;
		private readonly TextureAddressMode addressV;
		private readonly bool alphaBlendEnable;
		private readonly BlendFunction alphaBlendOperation;
		private readonly CompareFunction alphaFunction;
		private readonly bool alphaTestEnable;
		private readonly CullMode cullMode;
		private readonly bool depthBufferEnable;
		private readonly Blend destinationBlend;
		private readonly GraphicsDevice device;

		private readonly TextureFilter magFilter;
		private readonly int maxMipLevel;
		private readonly TextureFilter minFilter;
		private readonly TextureFilter mipFilter;

		private readonly float mipMapLevelOfDetailBias;
		private readonly int referenceAlpha;
		private readonly bool seperateAlphaBlendEnabled;
		private readonly Blend sourceBlend;

		public TemporaryState(GraphicsDevice gd)
		{
			this.device = gd;
			this.cullMode = gd.RenderState.CullMode;
			this.depthBufferEnable = gd.RenderState.DepthBufferEnable;

			this.alphaBlendEnable = gd.RenderState.AlphaBlendEnable;
			this.alphaBlendOperation = gd.RenderState.AlphaBlendOperation;
			this.sourceBlend = gd.RenderState.SourceBlend;
			this.destinationBlend = gd.RenderState.DestinationBlend;
			this.seperateAlphaBlendEnabled = gd.RenderState.SeparateAlphaBlendEnabled;

			this.alphaTestEnable = gd.RenderState.AlphaTestEnable;
			this.alphaFunction = gd.RenderState.AlphaFunction;
			this.referenceAlpha = gd.RenderState.ReferenceAlpha;

			this.addressU = gd.SamplerStates[0].AddressU;
			this.addressV = gd.SamplerStates[0].AddressV;

			this.magFilter = gd.SamplerStates[0].MagFilter;
			this.minFilter = gd.SamplerStates[0].MinFilter;
			this.mipFilter = gd.SamplerStates[0].MipFilter;

			this.mipMapLevelOfDetailBias = gd.SamplerStates[0].MipMapLevelOfDetailBias;
			this.maxMipLevel = gd.SamplerStates[0].MaxMipLevel;
		}

		#region IDisposable Members

		public void Dispose()
		{
			this.device.RenderState.CullMode = this.cullMode;
			this.device.RenderState.DepthBufferEnable = this.depthBufferEnable;

			this.device.RenderState.AlphaBlendEnable = this.alphaBlendEnable;
			this.device.RenderState.AlphaBlendOperation = this.alphaBlendOperation;
			this.device.RenderState.SourceBlend = this.sourceBlend;
			this.device.RenderState.DestinationBlend = this.destinationBlend;
			this.device.RenderState.SeparateAlphaBlendEnabled = this.seperateAlphaBlendEnabled;

			this.device.RenderState.AlphaTestEnable = this.alphaTestEnable;
			this.device.RenderState.AlphaFunction = this.alphaFunction;
			this.device.RenderState.ReferenceAlpha = this.referenceAlpha;

			this.device.SamplerStates[0].AddressU = this.addressU;
			this.device.SamplerStates[0].AddressV = this.addressV;

			this.device.SamplerStates[0].MagFilter = this.magFilter;
			this.device.SamplerStates[0].MinFilter = this.minFilter;
			this.device.SamplerStates[0].MipFilter = this.mipFilter;

			this.device.SamplerStates[0].MipMapLevelOfDetailBias = this.mipMapLevelOfDetailBias;
			this.device.SamplerStates[0].MaxMipLevel = this.maxMipLevel;
		}

		#endregion
	}
}