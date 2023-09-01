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

texture textureMap;

sampler2D textureSampler = sampler_state
{
    Texture = (textureMap);
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.Normal = mul(input.Normal, World);
    output.TexCoord = input.TexCoord;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(textureSampler, input.TexCoord);
    return color;

}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};