#define Iterations 128

float2 Pan;
float Zoom;
float Aspect;

float4 PS(float2 tex : TEXCOORD) : SV_TARGET
{

	float2 c = (tex - 0.5) * Zoom * float2(1, Aspect) - Pan;
	float2 v = 0;

	for (int n = 0; n < Iterations; n++)
	{
		v = float2(v.x * v.x - v.y * v.y, v.x * v.y * 2) + c;
	}

	float r = dot(v, v);
	return r > 1 ? 1 : saturate(r * float4(tex.x, tex.y, 1, 1));
}

technique MyTechnique
{
	pass 
	{
		Profile = 11;
		PixelShader = PS;
	}
}
