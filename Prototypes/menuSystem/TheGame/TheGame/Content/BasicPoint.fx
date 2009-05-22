uniform extern float4x4 World;
uniform extern float4x4 View;
uniform extern float4x4 Projection;

uniform extern float	ViewportHeight;

uniform extern texture	SpriteTexture;

// The current time, in seconds.
uniform extern float CurrentTime;

sampler Sampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderInput
{
    float4 Position	: POSITION0;
    float3 Velocity	: NORMAL0;
    float4 Color	: COLOR0;
    float  Size		: PSIZE;
    float2 Time		: TEXCOORD0;
    float2 Data		: TEXCOORD1;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color	: COLOR0;
    float Size		: PSIZE;
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

VertexShaderOutput CartesianVertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float age = CurrentTime - input.Time.x;
    float normalizedAge = saturate(age / input.Time.y);

    output.Color = input.Color * input.Data.y;
    output.Color.a *= normalizedAge * (1 - normalizedAge) * (1 - normalizedAge) * 6.7;
    
    input.Position += float4(input.Velocity * age, 0);

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    
    output.Size = lerp(input.Size, input.Data.x, normalizedAge) * Projection._m11 / output.Position.w * ViewportHeight / 2;

    return output;
}

//Coordinares: x, y, z = radius, angle, height 
VertexShaderOutput CylindricalVertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float age = CurrentTime - input.Time.x;
    float normalizedAge = saturate(age / input.Time.y);
    
    output.Color = input.Color * input.Data.y;
    output.Color.a *= normalizedAge * (1 - normalizedAge) * (1 - normalizedAge) * 6.7;
    
    float4 pos = input.Position + float4(input.Velocity * age, 0);
    
    input.Position.x = pos.x * sin(pos.y);
    input.Position.z = pos.x * cos(pos.y); 
    input.Position.y = pos.z;
    

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    
    output.Position = mul(viewPosition, Projection);
    
    output.Size = lerp(input.Size, input.Data.x, normalizedAge) * Projection._m11 / output.Position.w * ViewportHeight / 2;

    return output;
}

//Corrdinates: x, y, z = radius, theta, phi
VertexShaderOutput SphericalVertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float age = CurrentTime - input.Time.x;
    float normalizedAge = saturate(age / input.Time.y);
    
    output.Color = input.Color * input.Data.y;
    output.Color.a *= normalizedAge * (1 - normalizedAge) * (1 - normalizedAge) * 6.7;
    
    float4 pos = input.Position + float4(input.Velocity * age, 0);
    
    input.Position.x = pos.x * cos(pos.y) * sin(pos.z);
    input.Position.y = pos.x * sin(pos.y) * sin(pos.z);
    input.Position.z = pos.x * cos(pos.y);

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    
    output.Position = mul(viewPosition, Projection);
    
    output.Size = lerp(input.Size, input.Data.x, normalizedAge) * Projection._m11 / output.Position.w * ViewportHeight / 2;

    return output;
}

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
    float2 textureCoordinate = input.TexCoord;
    return tex2D(Sampler, textureCoordinate) * input.Color;
}

technique Cartesian
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_1_1 CartesianVertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

technique Cylindrical
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_1_1 CylindricalVertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

technique Spherical
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_1_1 SphericalVertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}