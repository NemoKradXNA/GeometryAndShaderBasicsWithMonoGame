#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix World : World;
matrix View : View;
matrix Projection : Projection;

texture textureMap;

sampler2D textureSampler = sampler_state
{
    Texture = (textureMap);
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear our output structure, ready to be populated.
    VertexShaderOutput output = (VertexShaderOutput) 0;

    // Our vertex starts in object space, that is to say,
    // it is relative to other values inside the object.
    float4 objectSpace = input.Position;
    
    // We need to move this data into world space, to do that
    // we simply multiply the object space value by the World matrix.
    float4 worldSpace = mul(objectSpace, World);
    
    // Now our value is in World space, we need to make it
    // relative to the viewer, so we move it into View space
    // by simply multiplying the world space value by the
    // View matrix.
    float4 viewSpace = mul(worldSpace, View);
    
    // Now, we need to get this value on the screen, so we need
    // to move it to screen space, again, this is simply done
    // by multiplying the view space values by the Projection matrix.
    float4 screenSpace = mul(viewSpace, Projection);
    
    // We can now pass this onto the graphics pipeline.
    output.Position = screenSpace;
    
    // Pass on the color and texcoord data.
    output.Color = input.Color;
    output.TexCoord = input.TexCoord;

    // Send our structure onto the graphics pipeline.
    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Read data from the texture sampler using the provided 
    // interpolated texcoord value for this fragment.
    float4 color = tex2D(textureSampler, input.TexCoord);
    
    // Send the color to the render target.
    return color * input.Color;
}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};