uniform extern float4x4 World;
uniform extern float4x4 View;
uniform extern float4x4 Projection;

uniform extern texture	SpriteTexture;
uniform extern float4   Color;

// The current time, in seconds.
uniform extern float CurrentTime;

sampler TextureSampler = sampler_state 
{ 
	texture = <SpriteTexture> ; 
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter=  LINEAR; 
	AddressU = mirror; 
	AddressV = mirror;
};

struct VertexShaderInput
{
    float4 Position	: POSITION0;
    float2 TexCoord	: TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
    float4 Color    : COLOR0;
};

struct PixelShaderInput
{
	float4 Color		: COLOR0;

	#ifdef XBOX
        float2 TexCoord : SPRITETEXCOORD;
    #else
        float2 TexCoord : TEXCOORD0;
    #endif
    
    
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.TexCoord = input.TexCoord;
    output.Color = Color;

    return output;
}

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
    //float2 textureCoordinate = input.TexCoord;
    return tex2D(TextureSampler, input.TexCoord) * input.Color;
}

technique PositionTexture
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
