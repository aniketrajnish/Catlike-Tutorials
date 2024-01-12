Shader "GPU Graph/GPU Point Surface" {
    Properties {
		    _Smoothness ("Smoothness", Range(0,1)) = 0.5
	    }

    SubShader {
        CGPROGRAM
        #pragma surface ConfigureSurface Standard fullforwardshadows
        #pragma instancing_options procedural:ConfigureProcedural
        #pragma surface ConfigureSurface Standard fullforwardshadows addshadow
        #pragma instancing_options assumeuniformscaling procedural:ConfigureProcedural
        #pragma editor_sync_compilation
        #pragma target 4.5        

        struct Input
        {
            float3 worldPos;
        };

        #include "PointGPU.hlsl"
        
        float _Smoothness;
        
    
        void ConfigureSurface(Input input, inout SurfaceOutputStandard surface)
        {
            surface.Albedo = saturate(input.worldPos * 0.5f + 0.5f);
            surface.Smoothness = _Smoothness;
        }

        ENDCG
    }

    Fallback "Diffuse"
}