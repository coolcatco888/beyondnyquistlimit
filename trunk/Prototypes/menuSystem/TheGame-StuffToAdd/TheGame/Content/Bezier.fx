uniform extern float4x4 World;
uniform extern float4x4 View;
uniform extern float4x4 Projection;

uniform extern float	ViewportHeight;

uniform extern texture	SpriteTexture;

// The current time, in seconds.
uniform extern float CurrentTime;

uniform extern float3 ControlPoint0;
uniform extern float3 ControlPoint1;
uniform extern float3 ControlPoint2;
uniform extern float3 ControlPoint3;

uniform extern float PI = 3.14159;
uniform extern float3 Up = float3(0, 1, 0);

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

float3 CubicBezier(float3 control0, float3 control1, float3 control2, float3 control3, float t)
{
	float3 position = pow((1-t),3) * control0 + 3*pow((1-t),2)*t * control1 + 3*(1-t)*pow(t,2) * control2 + pow(t,3) * control3;
	
	return position;
}

float3 CubicBezierDerivitive(float3 control0, float3 control1, float3 control2, float3 control3, float t)
{
	float3 direction = -3 * pow((1-t),2) * control0 + 3*pow((1-t),2) * control1 - 6*(1-t)*t * control1 + 6*(1-t)*t * control2 - 3*pow(t,2) * control2 + 3*pow(t,2) * control3;
	return normalize(direction);
}

VertexShaderOutput BezierVertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float age = CurrentTime - input.Time.x;
    float normalizedAge = saturate(age / input.Time.y);
    
    float t = saturate(input.Data.x + input.Data.y * age);
    
    float3 pos = CubicBezier(ControlPoint0, ControlPoint1, ControlPoint2, ControlPoint3, t);
    float3 tangent = CubicBezierDerivitive(ControlPoint0, ControlPoint1, ControlPoint2, ControlPoint3, t);
    
    float4x4 Rot90 =
	{
		1, 0		,  0		, 0,
		0, cos(PI/2), -sin(PI/2), 0,
		0, sin(PI/2),  cos(PI/2), 0,
		0, 0		,  0		, 1
	};
	
	float3 n = normalize(Up - dot(Up, tangent) * tangent);

	float3 n2 = cross(tangent, n);
	
	float4 scale = input.Position + float4(input.Velocity, 0) * age;

    output.Color = input.Color;
    output.Color.a *= normalizedAge * (1 - normalizedAge) * (1 - normalizedAge) * 6.7;
    
    //input.Position = float4(pos + (scale.x * n2), input.Position.w);
    input.Position = float4(pos + (scale.x * cos(scale.y) * n + scale.x * sin(scale.y) * n2 + scale.z * tangent), input.Position.w);

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    
    output.Size = input.Size * Projection._m11 / output.Position.w * ViewportHeight / 2;

    return output;
}

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
    float2 textureCoordinate = input.TexCoord;
    return tex2D(Sampler, textureCoordinate) * input.Color * 2;
}

technique Bezier
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_1_1 BezierVertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
