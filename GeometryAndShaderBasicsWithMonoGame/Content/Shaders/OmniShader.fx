#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix World;
matrix WorldViewProjection;
float3 lightDirection;

float3 CameraPosition;
float uvMultiplier = 1;

float4 AmbientColor = float4(.5, .5, .5, 1);
float AmbientPower = .5f;


texture textureMap;
texture normalMap;
texture occlusionMap;
texture specularMap;

sampler textureSample = sampler_state
{
    texture = <textureMap>;
    AddressU = Wrap;
    AddressV = Wrap;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

sampler NormalMapSampler = sampler_state
{
    Texture = <normalMap>;
    AddressU = Wrap;
    AddressV = Wrap;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

sampler OcclusionSampler = sampler_state
{
    Texture = <occlusionMap>;
    AddressU = Wrap;
    AddressV = Wrap;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

sampler SpecularSampler = sampler_state
{
    Texture = <specularMap>;
    AddressU = Wrap;
    AddressV = Wrap;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};



struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
    float3 Normal : NORMAL0;
    float3 TexCoord : TEXCOORD0;
    float3 Tangent : TANGENT0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TexCoord0;
    float3 Normal : Normal0;
    float3x3 Tangent : Tangent0;
    float3 CamView : TEXCOORD1;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.TexCoord = input.TexCoord * uvMultiplier;
        
    output.Normal = mul(input.Normal, World);
    
    output.Tangent[0] = normalize(mul(input.Tangent, World));
    output.Tangent[1] = normalize(mul(cross(input.Tangent, input.Normal), World));
    output.Tangent[2] = normalize(output.Normal);
    
    output.CamView = CameraPosition - mul(input.Position, World).xyz;
    
    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    
    //input.TexCoord = ApplyParallaxOffset(input.TexCoord, input.Normal, float2(1, 1));
    float4 Color = tex2D(textureSample, input.TexCoord);
    
    float o = tex2D(OcclusionSampler, input.TexCoord);
    
    Color *= o * Color.a;
    
	// Get value in the range of -1 to 1
    float3 n = 2.0f * tex2D(NormalMapSampler, input.TexCoord) - 1.0f;
    n = mul(n, input.Tangent) ;

    float3 lightVector = normalize(lightDirection);

    float NdL = saturate(dot(lightVector, n));
    
    float3 Half = lightVector + normalize(input.CamView);
    float specular = pow(saturate(dot(n, Half)), 2);
    float s = tex2D(SpecularSampler, input.TexCoord).r;
    
    specular *= s;

    return (AmbientColor * AmbientPower) + ((Color + specular) * NdL);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};