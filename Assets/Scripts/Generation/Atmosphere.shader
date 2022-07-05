

Shader "Unlit/Atmosphere"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Position ("Position", Vector) = (0,0,0,0) 
        _Radius ("Radius", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 _Position;
            float _Radius;
            
            struct Ray
            {
                float3 origin;
                float3 direction;
            };

            Ray createRay(float3 origin, float3 direction)
            {
                Ray ray;
                ray.origin = origin;
                ray.direction = direction;
                return ray;
            }

            

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 viewVector : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            Ray createCameraRay(float2 uv)
            {
                // Transform the camera origin to world space
                float3 origin = mul(unity_CameraToWorld, float4(0.0f, 0.0f, 0.0f, 1.0f)).xyz;
    
                // Invert the perspective projection of the view-space position
                float3 direction = mul(unity_CameraInvProjection, float4(uv, 0.0f, 1.0f)).xyz;
                // Transform the direction from camera to world space and normalize
                direction = mul(unity_CameraToWorld, float4(direction, 0.0f)).xyz;
                direction = normalize(direction);

                return createRay(origin, direction);
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture

                Ray ray = createCameraRay(i.uv);
                
                float rayDistance = dot(_Position - ray.origin, ray.direction);
                float3 intersectionToSphereEdge = ray.origin + ray.direction * rayDistance;
                float intersectionToSphereCenterDistance = length(_Position - intersectionToSphereEdge);
                int inside = intersectionToSphereEdge <= _Radius ? 0 : 1;
                
                float4 col = inside;
                
                return col;
            }
            ENDCG
        }
    }
}
