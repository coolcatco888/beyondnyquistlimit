uniform extern float4x4 World;
uniform extern float4x4 View;
uniform extern float4x4 Projection;

uniform extern float	ViewportHeight;

uniform extern texture	SpriteTexture;
uniform extern float	ParticleSize;

// The current time, in seconds.
uniform extern float CurrentTime;
uniform extern float Duration;

sampler Sampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderInput
{
    float4 Position	: POSITION0;
    float3 Velocity	: NORMAL0;
    float4 Color	: COLOR0;
    float  Birth	: TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color	: COLOR0;
    float Size		: PSIZE;
};

struct PixelShaderInput
{
	#ifdef XBOX
        float2 TexCoord : SPRITETEXCOORD;
    #else
        float2 TexCoord : TEXCOORD0;
    #endif
    
    float4 Color		: COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float age = CurrentTime - input.Birth;
    float normalizedAge = saturate(age / Duration);
    
    output.Color = input.Color;
    output.Color.a *= normalizedAge * (1 - normalizedAge) * (1 - normalizedAge) * 6.7;
    
    input.Position += float4(input.Velocity * age, 1);

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    
    output.Position = mul(viewPosition, Projection);
    
    output.Size = ParticleSize * Projection._m11 / output.Position.w * ViewportHeight / 2;

    return output;
}

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
    float2 texCoord;

    texCoord = input.TexCoord.xy;

    return tex2D(Sampler, texCoord) * input.Color * 2;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
