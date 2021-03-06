﻿float4x4 wvp;
float4x4 world;

float4 ambientColor = float4(0.2, 0.2, 0.2, 1);
float3 lightPosition = float3(0, 200.0, 50);

Texture2D tex;
SamplerState TextureSampler;

struct VSInput
{
	float4 Position : SV_POSITION;
	float2 Texture : TEXCOORD0;
	float3 Normal : NORMAL;
};

struct PSInput
{
	float4 Position : SV_POSITION;
	float2 Texture : TEXCOORD0;
	float3 Pos : TEXCOORD1;
    float3 Normal : NORMAL;
};

float4 PS(PSInput input) : SV_TARGET
{
	float lightIntensity;
	float4 color;
		
	float4 diffuseColor = tex.Sample(TextureSampler, float2(input.Texture.x % 1, input.Texture.y % 1));

	color = ambientColor * diffuseColor;

	lightIntensity = saturate(dot(input.Normal, normalize(lightPosition - input.Pos.xyz)));

	color += diffuseColor * lightIntensity;

	color = saturate(color);

	return color;
}

PSInput VS(VSInput input)
{
	PSInput psi;

	psi.Position = mul(input.Position, wvp);
	psi.Texture = input.Texture / 4;
	psi.Normal = normalize(mul(input.Normal, world).xyz);
	psi.Pos = mul(input.Position, world).xyz;

	return psi;
}

technique BasicTechnique
{
	pass 
	{
		Profile = 11;
		PixelShader = PS;
		VertexShader = VS;
	}
}
