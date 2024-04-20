Shader "Hidden/PaperShader"
{
    Properties{
        _MainTex ("Texture", 2D) = "white" {}
        _BeforeTex("Before Texture",2D)= "black" {}
        _PaperTex ("PaperTexture", 2D) = "white" {}
        [MaterialToggle] _Reverse ("Reverse", int) = 0 
        _Flip("Flip",Range(-1, 1)) = 0
    }

    SubShader
    {
        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertexOS : POSITION;
                float2 texcoord : TEXCOORD0;
            }; 

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            
            float l2(float x,float flip)
			{
				return 1 - flip + 0.1 * cos(x * 1.5);
			}

			float l1(float y,float flip)
			{
				return flip + 0.1 * sin(y * 3);
			}

			float l0(float x,float flip)
			{
				return x - flip;
			}

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;

                o.pos = UnityObjectToClipPos(v.vertexOS);
                
                o.uv = v.texcoord;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _BeforeTex;
            sampler2D _PaperTex;
            bool _Reverse;
            float _Flip;

           
            half4 frag (v2f i) : SV_Target
            {
                float2 texUV = i.uv;
                float2 beforeTexUV = i.uv;
                #if UNITY_UV_STARTS_AT_TOP
                texUV.y = 1 - texUV.y;
                #endif

                
                half4 col = _Reverse?tex2D(_MainTex, texUV):tex2D(_BeforeTex, beforeTexUV);

                float flip = 1 - (1-_Flip)*2;


                if((i.uv.y<(i.uv.x-flip+1*(1-flip)))){
                    col *= min(float4(0.99, 0.99, 0.99, 1),(i.uv.y-i.uv.x)+2*(flip));
                   
                }

				//範囲内ならば暗い色に
				if (i.uv.x > l1(i.uv.y,flip) && i.uv.y < l2(i.uv.x,flip)){
					col = float4(0.5, 0.5, 0.5, 1)*tex2D(_PaperTex, i.uv);
                }                    
                
				//L0より右の描画を無視
				float l0_y = l0(i.uv.x,flip);
				if(i.uv.y - l0_y<=0){
                    col= _Reverse?tex2D(_BeforeTex, beforeTexUV):tex2D(_MainTex, texUV);
                    if(i.uv.y - l0_y>-0.15&&abs(_Flip)>0.05){
                        col*=min((0.1+(-i.uv.y + l0_y)*6),0.99);
                    }
                }

                
                
                return col;
            }

            ENDHLSL
        }
    }
}
