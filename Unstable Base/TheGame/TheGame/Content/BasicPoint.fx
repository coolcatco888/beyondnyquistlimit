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
    float4 Rotation : COLOR1;
    float Size		: PSIZE;
};

struct PixelShaderInput
{
	float4 Color		: COLOR0;
	float4 Rotation : COLOR1;

	#ifdef XBOX
        float2 TexCoord : SPRITETEXCOORD;
    #else
        float2 TexCoord : TEXCOORD0;
    #endif
    
    
};

// Vertex shader helper for computing the rotation of a particle.
float4 ComputeParticleRotation(float speed, float age)
{    
    
    float rotation = speed * age;

    // Compute a 2x2 rotation matrix.
    float c = cos(rotation);
    float s = sin(rotation);
    
    float4 rotationMatrix = float4(c, -s, s, c);
    
    // Normally we would output this matrix using a texture coordinate interpolator,
    // but texture coordinates are generated directly by the hardware when drawing
    // point sprites. So we have to use a color interpolator instead. Only trouble
    // is, color interpolators are clamped to the range 0 to 1. Our rotation values
    // range from -1 to 1, so we have to scale them to avoid unwanted clamping.
    
    rotationMatrix *= 0.5;
    rotationMatrix += 0.5;
    
    return rotationMatrix;
}

VertexShaderOutput CartesianVertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float age = CurrentTime - input.Time.x;
    float normalizedAge = saturate(age / input.Time.y);
    
    output.Rotation = ComputeParticleRotation(input.Data.x, age);

    output.Color = input.Color;
    output.Color.a *= normalizedAge * (1 - normalizedAge) * (1 - normalizedAge) * 6.7;
    
    input.Position += float4(input.Velocity * age, 0);

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    
    output.Size = input.Size * Projection._m11 / output.Position.w * ViewportHeight / 2;

    return output;
}

//Coordinares: x, y, z = radius, angle, height 
VertexShaderOutput CylindricalVertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float age = CurrentTime - input.Time.x;
    float normalizedAge = saturate(age / input.Time.y);
    
    output.Rotation = ComputeParticleRotation(input.Data.x, age);
    
    output.Color = input.Color;
    output.Color.a *= normalizedAge * (1 - normalizedAge) * (1 - normalizedAge) * 6.7;
    
    float4 pos = input.Position + float4(input.Velocity * age, 0);
    
    input.Position.x = pos.x * sin(pos.y);
    input.Position.z = pos.x * cos(pos.y); 
    input.Position.y = pos.z;
    

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    
    output.Position = mul(viewPosition, Projection);
    
    output.Size = input.Size * Projection._m11 / output.Position.w * ViewportHeight / 2;

    return output;
}

//Corrdinates: x, y, z = radius, theta, phi
VertexShaderOutput SphericalVertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float age = CurrentTime - input.Time.x;
    float normalizedAge = saturate(age / input.Time.y);
    
    output.Rotation = ComputeParticleRotation(input.Data.x, age);
    
    output.Color = input.Color;
    output.Color.a *= normalizedAge * (1 - normalizedAge) * (1 - normalizedAge) * 6.7;
    
    float4 pos = input.Position + float4(input.Velocity * age, 0);
    
    input.Position.x = pos.x * cos(pos.y) * sin(pos.z);
    input.Position.y = pos.x * sin(pos.y) * sin(pos.z);
    input.Position.z = pos.x * cos(pos.y);

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    
    output.Position = mul(viewPosition, Projection);
    
    output.Size = input.Size * Projection._m11 / output.Position.w * ViewportHeight / 2;

    return output;
}

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
    float2 textureCoordinate = input.TexCoord;

    // We want to rotate around the middle of the particle, not the origin,
    // so we offset the texture coordinate accordingly.
    //textureCoordinate -= 0.5;
    
    // Apply the rotation matrix, after rescaling it back from the packed
    // color interpolator format into a full -1 to 1 range.
    //float4 rotation = input.Rotation * 2 - 1;
    
    //textureCoordinate = mul(textureCoordinate, float2x2(rotation));
    
    // Point sprites are squares. So are textures. When we rotate one square
    // inside another square, the corners of the texture will go past the
    // edge of the point sprite and get clipped. To avoid this, we scale
    // our texture coordinates to make sure the entire square can be rotated
    // inside the point sprite without any clipping.
    //textureCoordinate *= sqrt(2);
    
    // Undo the offset used to control the rotation origin.
    //textureCoordinate += 0.5;

    return tex2D(Sampler, textureCoordinate) * input.Color * 2;
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