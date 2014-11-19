//#EditorFriendly
//#node19:posx=-571:posy=414.5:title=ParamFloat:title2=X:input0=1:input0type=float:
//#node18:posx=-578:posy=257.5:title=ParamFloat:title2=Y:input0=1:input0type=float:
//#node17:posx=-590:posy=85.5:title=ParamFloat:title2=B:input0=1:input0type=float:
//#node16:posx=-602:posy=-65.5:title=ParamFloat:title2=A:input0=1:input0type=float:
//#node15:posx=-172:posy=59.5:title=Add:input0=0:input0type=float:input0linkindexnode=14:input0linkindexoutput=0:input1=0:input1type=float:input1linkindexnode=12:input1linkindexoutput=0:
//#node14:posx=-268:posy=-13.5:title=Add:input0=0:input0type=float:input0linkindexnode=13:input0linkindexoutput=0:input1=0:input1type=float:input1linkindexnode=11:input1linkindexoutput=0:
//#node13:posx=-359:posy=-73.5:title=Add:input0=0:input0type=float:input0linkindexnode=9:input0linkindexoutput=0:input1=0:input1type=float:input1linkindexnode=10:input1linkindexoutput=0:
//#node12:posx=-408:posy=350.5:title=Multiply:input0=1:input0type=float:input0linkindexnode=8:input0linkindexoutput=0:input1=1:input1type=float:input1linkindexnode=19:input1linkindexoutput=0:
//#node11:posx=-432:posy=221.5:title=Multiply:input0=1:input0type=float:input0linkindexnode=7:input0linkindexoutput=0:input1=1:input1type=float:input1linkindexnode=18:input1linkindexoutput=0:
//#node10:posx=-463:posy=4.5:title=Multiply:input0=1:input0type=float:input0linkindexnode=6:input0linkindexoutput=0:input1=1:input1type=float:input1linkindexnode=17:input1linkindexoutput=0:
//#node9:posx=-470:posy=-84.5:title=Multiply:input0=1:input0type=float:input0linkindexnode=5:input0linkindexoutput=0:input1=1:input1type=float:input1linkindexnode=16:input1linkindexoutput=0:
//#node8:posx=-556:posy=332.5:title=Texture:title2=TX:input0=(0,0):input0type=Vector2:
//#node7:posx=-588:posy=164.5:title=Texture:title2=TY:input0=(0,0):input0type=Vector2:
//#node6:posx=-590:posy=12.5:title=Texture:title2=TB:input0=(0,0):input0type=Vector2:
//#node5:posx=-584:posy=-133.5:title=Texture:title2=TA:input0=(0,0):input0type=Vector2:
//#node4:posx=0:posy=0:title=Lighting:title2=On:
//#node3:posx=0:posy=0:title=DoubleSided:title2=Back:
//#node2:posx=0:posy=0:title=FallbackInfo:title2=Transparent/Cutout/VertexLit:input0=1:input0type=float:
//#node1:posx=0:posy=0:title=LODInfo:title2=LODInfo1:input0=600:input0type=float:
//#masterNode:posx=0:posy=0:title=Master Node:input1linkindexnode=15:input1linkindexoutput=0:input4linkindexnode=15:input4linkindexoutput=0:
//#sm=3.0
//#blending=Normal
//#ShaderName
Shader "ShaderFusion/shader_boutons" {
	Properties {
_Color ("Diffuse Color", Color) = (1.0, 1.0, 1.0, 1.0)
_SpecColor ("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
//#ShaderProperties
_TA ("TA", 2D) = "white" {}
_A ("A", Float) = 1
_TB ("TB", 2D) = "white" {}
_B ("B", Float) = 1
_TY ("TY", 2D) = "white" {}
_Y ("Y", Float) = 1
_TX ("TX", 2D) = "white" {}
_X ("X", Float) = 1
	}
	Category {
		SubShader { 
//#Blend
ZWrite On
//#CatTags
Tags { "RenderType"="Opaque" "Queue"="Overlay"}
Lighting On
Cull Back
//#LOD
LOD 600

Ztest always
//#GrabPass
		CGPROGRAM
//#LightingModelTag
#pragma surface surf ShaderFusion vertex:vert alphatest:_Cutoff
 //use custom lighting functions
 
 //custom surface output structure
 struct SurfaceShaderFusion {
	half3 Albedo;
	half3 Normal;
	half3 Emission;
	half Specular;
	half3 GlossColor; //Gloss is now three-channel
	half Alpha;
 };
inline fixed4 LightingShaderFusion (SurfaceShaderFusion s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	half3 h = normalize (lightDir + viewDir);
	
	fixed diff = max (0, dot (s.Normal, lightDir));
	
	float nh = max (0, dot (s.Normal, h));
	float3 spec = pow (nh, s.Specular*128.0) * s.GlossColor;
	
	fixed4 c;
	c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * _SpecColor.rgb * spec) * (atten * 2);
	c.a = s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten;
	return c;
}
inline fixed4 LightingShaderFusion_PrePass (SurfaceShaderFusion s, half4 light)
{
	fixed3 spec = light.a * s.GlossColor;
	
	fixed4 c;
	c.rgb = (s.Albedo * light.rgb + light.rgb * _SpecColor.rgb * spec);
	c.a = s.Alpha + spec * _SpecColor.a;
	return c;
}
inline half4 LightingShaderFusion_DirLightmap (SurfaceShaderFusion s, fixed4 color, fixed4 scale, half3 viewDir, bool surfFuncWritesNormal, out half3 specColor)
{
	UNITY_DIRBASIS
	half3 scalePerBasisVector;
	
	half3 lm = DirLightmapDiffuse (unity_DirBasis, color, scale, s.Normal, surfFuncWritesNormal, scalePerBasisVector);
	
	half3 lightDir = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
	half3 h = normalize (lightDir + viewDir);
	float nh = max (0, dot (s.Normal, h));
	float spec = pow (nh, s.Specular * 128.0);
	
	// specColor used outside in the forward path, compiled out in prepass
	specColor = lm * _SpecColor.rgb * s.GlossColor * spec;
	
	// spec from the alpha component is used to calculate specular
	// in the Lighting*_Prepass function, it's not used in forward
	return half4(lm, spec);
}
//#TargetSM
#pragma target 3.0
//#UnlitCGDefs
sampler2D _TA;
float _A;
sampler2D _TB;
float _B;
sampler2D _TY;
float _Y;
sampler2D _TX;
float _X;
float4 _Color;
		struct Input {
//#UVDefs
float2 sfuv1;
		INTERNAL_DATA
		};
		
		void vert (inout appdata_full v, out Input o) {
//#DeferredVertexBody
o.sfuv1 = v.texcoord.xy;
//#DeferredVertexEnd
		}
		void surf (Input IN, inout SurfaceShaderFusion o) {
			float4 normal = float4(0.0,0.0,1.0,0.0);
			float3 emissive = 0.0;
			float3 specular = 1.0;
			float gloss = 1.0;
			float3 diffuse = 1.0;
			float alpha = 1.0;
//#PreFragBody
float4 node5 = tex2D(_TA,IN.sfuv1);
float4 node6 = tex2D(_TB,IN.sfuv1);
float4 node7 = tex2D(_TY,IN.sfuv1);
float4 node8 = tex2D(_TX,IN.sfuv1);
//#FragBody
alpha = (((((node5) * (_A)) + ((node6) * (_B))) + ((node7) * (_Y))) + ((node8) * (_X)));
emissive = (((((node5) * (_A)) + ((node6) * (_B))) + ((node7) * (_Y))) + ((node8) * (_X)));
			
			o.Albedo = diffuse.rgb*_Color;
			#ifdef SHADER_API_OPENGL
			o.Albedo = max(float3(0,0,0),o.Albedo);
			#endif
			
			o.Emission = emissive*_Color;
			#ifdef SHADER_API_OPENGL
			o.Emission = max(float3(0,0,0),o.Emission);
			#endif
			
			o.GlossColor = specular*_SpecColor;
			#ifdef SHADER_API_OPENGL
			o.GlossColor = max(float3(0,0,0),o.GlossColor);
			#endif
			
			o.Alpha = sqrt(dot(emissive, emissive));//alpha*_Color.a;
			#ifdef SHADER_API_OPENGL
			o.Alpha = max(float3(0,0,0),o.Alpha);
			#endif
			
			o.Specular = gloss;
			#ifdef SHADER_API_OPENGL
			o.Specular = max(float3(0,0,0),o.Specular);
			#endif
			
			o.Normal = normal;
//#FragEnd
		}
		ENDCG
		}
	}
//#Fallback
Fallback "Transparent/Cutout/VertexLit"
}
