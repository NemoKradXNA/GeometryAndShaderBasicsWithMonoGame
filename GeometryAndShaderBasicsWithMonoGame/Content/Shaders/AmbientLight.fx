#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection : WorldViewProjection;
float AmbientIntensity = 1;
float4 AmbientColor : AMBIENT = float4(.5, .5, .5, 1);

texture textureMap;

sampler2D textureSampler = sampler_state
{
    Texture = (textureMap);
};

struct VS_IN
{
    float4 Position : POSITION;
    float2 TexCoord : TEXCOORD0;
};

struct VS_OUT
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
};

VS_OUT VS_Ambient(VS_IN input)
{
    VS_OUT output = (VS_OUT) 0;
	
    output.Position = mul(input.Position, WorldViewProjection);
    output.TexCoord = input.TexCoord;
    
    return output;
}

float4 PS_Ambient(VS_OUT input) : COLOR
{
    float4 color = tex2D(textureSampler, input.TexCoord);
   
    color *= AmbientIntensity * AmbientColor;
	
    return color;
}

technique AmbientLighting
{
	pass P0
	{
        VertexShader = compile VS_SHADERMODEL VS_Ambient();
        PixelShader = compile PS_SHADERMODEL PS_Ambient();
    }
};