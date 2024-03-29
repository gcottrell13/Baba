﻿#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define RADIUS  7
#define KERNEL_SIZE (RADIUS * 2 + 1)

//-----------------------------------------------------------------------------
// Globals.
//-----------------------------------------------------------------------------

float weights[KERNEL_SIZE];
float2 offsets[KERNEL_SIZE];
Texture2D SpriteTexture;

sampler s0;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 texCoord : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);

	for (int i = 0; i < KERNEL_SIZE; ++i)
		color += tex2D(s0, input.texCoord + offsets[i]) * weights[i];

	return tex2D(SpriteTextureSampler, input.texCoord) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};