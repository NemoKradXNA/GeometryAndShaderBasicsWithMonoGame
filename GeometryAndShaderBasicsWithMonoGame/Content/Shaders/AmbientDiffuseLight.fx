#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix World : World;
matrix WorldViewProjection : WorldViewProjection;

float AmbientIntensity = 1;
float4 AmbientColor : Ambient = float4(.5, .5, .5, 1);
float3 lightDirection : Direction = float3(0, 50, 10);

texture textureMap;

sampler2D textureSampler = sampler_state
{
    Texture = (textureMap);
};

struct VS_IN
{
    float4 Position : POSITION;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
};

struct VS_OUT
{
    float4 Position : POSITION;
    float3 Light : TEXCOORD1;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
};



VS_OUT VS_AmbientDiffuse(VS_IN input)
{
    VS_OUT output = (VS_OUT) 0;
	
    output.Position = mul(input.Position, WorldViewProjection);
    output.Light = normalize(lightDirection);
    output.Normal = normalize(mul(input.Normal, World));
    output.TexCoord = input.TexCoord;
    return output;
}

float4 PS_AmbientDiffuse(VS_OUT input) : COLOR
{
    float4 color = tex2D(textureSampler, input.TexCoord);
	
    color = (AmbientIntensity * AmbientColor) + (color * saturate(dot(input.Light, input.Normal)));
	
    return color;
}

technique AmbientDiffusedLighting
{
	pass P0
	{
        VertexShader = compile VS_SHADERMODEL VS_AmbientDiffuse();
        PixelShader = compile PS_SHADERMODEL PS_AmbientDiffuse();
    }
};