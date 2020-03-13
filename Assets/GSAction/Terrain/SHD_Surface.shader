Shader "Custom/Surface" {
    Properties {
		_MainTex ("Transparent, will change color", 2D) = ""
	}
	 
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent+1" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
        ZTest Always
        LOD 100
        
        Pass {
            Lighting Off
            
            SetTexture [_MainTex] { 
				
            }
        }
	}
}