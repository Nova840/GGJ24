Shader "Unlit/NewUnlitShader"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Pass
		{
			Blend One One
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};
#define hlsl_atan(x,y) atan2(x, y)
#define mod(x,y) ((x)-(y)*floor((x)/(y)))
inline float4 textureLod(sampler2D tex, float2 uv, float lod) {
    return tex2D(tex, uv);
}
inline float2 tofloat2(float x) {
    return float2(x, x);
}
inline float2 tofloat2(float x, float y) {
    return float2(x, y);
}
inline float3 tofloat3(float x) {
    return float3(x, x, x);
}
inline float3 tofloat3(float x, float y, float z) {
    return float3(x, y, z);
}
inline float3 tofloat3(float2 xy, float z) {
    return float3(xy.x, xy.y, z);
}
inline float3 tofloat3(float x, float2 yz) {
    return float3(x, yz.x, yz.y);
}
inline float4 tofloat4(float x, float y, float z, float w) {
    return float4(x, y, z, w);
}
inline float4 tofloat4(float x) {
    return float4(x, x, x, x);
}
inline float4 tofloat4(float x, float3 yzw) {
    return float4(x, yzw.x, yzw.y, yzw.z);
}
inline float4 tofloat4(float2 xy, float2 zw) {
    return float4(xy.x, xy.y, zw.x, zw.y);
}
inline float4 tofloat4(float3 xyz, float w) {
    return float4(xyz.x, xyz.y, xyz.z, w);
}
inline float4 tofloat4(float2 xy, float z, float w) {
    return float4(xy.x, xy.y, z, w);
}
inline float2x2 tofloat2x2(float2 v1, float2 v2) {
    return float2x2(v1.x, v1.y, v2.x, v2.y);
}
// EngineSpecificDefinitions
float rand(float2 x) {
    return frac(cos(mod(dot(x, tofloat2(13.9898, 8.141)), 3.14)) * 43758.5453);
}
float2 rand2(float2 x) {
    return frac(cos(mod(tofloat2(dot(x, tofloat2(13.9898, 8.141)),
						      dot(x, tofloat2(3.4562, 17.398))), tofloat2(3.14))) * 43758.5453);
}
float3 rand3(float2 x) {
    return frac(cos(mod(tofloat3(dot(x, tofloat2(13.9898, 8.141)),
							  dot(x, tofloat2(3.4562, 17.398)),
                              dot(x, tofloat2(13.254, 5.867))), tofloat3(3.14))) * 43758.5453);
}
float param_rnd(float minimum, float maximum, float seed) {
	return minimum+(maximum-minimum)*rand(tofloat2(seed));
}
float3 rgb2hsv(float3 c) {
	float4 K = tofloat4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = c.g < c.b ? tofloat4(c.bg, K.wz) : tofloat4(c.gb, K.xy);
	float4 q = c.r < p.x ? tofloat4(p.xyw, c.r) : tofloat4(c.r, p.yzx);
	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return tofloat3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}
float3 hsv2rgb(float3 c) {
	float4 K = tofloat4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
float wave_constant(float x) {
	return 1.0;
}
float wave_sine(float x) {
	return 0.5-0.5*cos(3.14159265359*2.0*x);
}
float wave_triangle(float x) {
	x = frac(x);
	return min(2.0*x, 2.0-2.0*x);
}
float wave_sawtooth(float x) {
	return frac(x);
}
float wave_square(float x) {
	return (frac(x) < 0.5) ? 0.0 : 1.0;
}
float wave_bounce(float x) {
	x = 2.0*(frac(x)-0.5);
	return sqrt(1.0-x*x);
}
float lerp_mul(float x, float y) {
	return x*y;
}
float lerp_add(float x, float y) {
	return min(x+y, 1.0);
}
float lerp_max(float x, float y) {
	return max(x, y);
}
float lerp_min(float x, float y) {
	return min(x, y);
}
float lerp_xor(float x, float y) {
	return min(x+y, 2.0-x-y);
}
float lerp_pow(float x, float y) {
	return pow(x, y);
}
float beehive_dist(float2 p){
	float2 s = tofloat2(1.0, 1.73205080757);
	p = abs(p);
	return max(dot(p, s*.5), p.x);
}
float4 beehive_center(float2 p) {
	float2 s = tofloat2(1.0, 1.73205080757);
	float4 hC = floor(tofloat4(p, p - tofloat2(.5, 1))/tofloat4(s,s)) + .5;
	float4 h = tofloat4(p - hC.xy*s, p - (hC.zw + .5)*s);
	return dot(h.xy, h.xy)<dot(h.zw, h.zw) ? tofloat4(h.xy, hC.xy) : tofloat4(h.zw, hC.zw + 9.73);
}
float pingpong(float a, float b)
{
  return (b != 0.0) ? abs(frac((a - b) / (b * 2.0)) * b * 2.0 - b) : 0.0;
}
uniform sampler2D texture_1;
static const float texture_1_size = 128.0;
float2 transform2_clamp(float2 uv) {
	return clamp(uv, tofloat2(0.0), tofloat2(1.0));
}
float2 transform2(float2 uv, float2 translate, float rotate, float2 scale) {
 	float2 rv;
	uv -= translate;
	uv -= tofloat2(0.5);
	rv.x = cos(rotate)*uv.x + sin(rotate)*uv.y;
	rv.y = -sin(rotate)*uv.x + cos(rotate)*uv.y;
	rv /= scale;
	rv += tofloat2(0.5);
	return rv;	
}
float3 blend_normal(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*c1 + (1.0-opacity)*c2;
}
float3 blend_dissolve(float2 uv, float3 c1, float3 c2, float opacity) {
	if (rand(uv) < opacity) {
		return c1;
	} else {
		return c2;
	}
}
float3 blend_multiply(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*c1*c2 + (1.0-opacity)*c2;
}
float3 blend_screen(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*(1.0-(1.0-c1)*(1.0-c2)) + (1.0-opacity)*c2;
}
float blend_overlay_f(float c1, float c2) {
	return (c1 < 0.5) ? (2.0*c1*c2) : (1.0-2.0*(1.0-c1)*(1.0-c2));
}
float3 blend_overlay(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_overlay_f(c1.x, c2.x), blend_overlay_f(c1.y, c2.y), blend_overlay_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float3 blend_hard_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*0.5*(c1*c2+blend_overlay(uv, c1, c2, 1.0)) + (1.0-opacity)*c2;
}
float blend_soft_light_f(float c1, float c2) {
	return (c2 < 0.5) ? (2.0*c1*c2+c1*c1*(1.0-2.0*c2)) : 2.0*c1*(1.0-c2)+sqrt(c1)*(2.0*c2-1.0);
}
float3 blend_soft_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_soft_light_f(c1.x, c2.x), blend_soft_light_f(c1.y, c2.y), blend_soft_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_burn_f(float c1, float c2) {
	return (c1==0.0)?c1:max((1.0-((1.0-c2)/c1)),0.0);
}
float3 blend_burn(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_burn_f(c1.x, c2.x), blend_burn_f(c1.y, c2.y), blend_burn_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_dodge_f(float c1, float c2) {
	return (c1==1.0)?c1:min(c2/(1.0-c1),1.0);
}
float3 blend_dodge(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_dodge_f(c1.x, c2.x), blend_dodge_f(c1.y, c2.y), blend_dodge_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float3 blend_lighten(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*max(c1, c2) + (1.0-opacity)*c2;
}
float3 blend_darken(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*min(c1, c2) + (1.0-opacity)*c2;
}
float3 blend_difference(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*clamp(c2-c1, tofloat3(0.0), tofloat3(1.0)) + (1.0-opacity)*c2;
}
float3 blend_additive(float2 uv, float3 c1, float3 c2, float oppacity) {
	return c2 + c1 * oppacity;
}
float3 blend_addsub(float2 uv, float3 c1, float3 c2, float oppacity) {
	return c2 + (c1 - .5) * 2.0 * oppacity;
}
float blend_linear_light_f(float c1, float c2) {
	return (c1 + 2.0 * c2) - 1.0;
}
float3 blend_linear_light(float2 uv, float3 c1, float3 c2, float opacity) {
return opacity*tofloat3(blend_linear_light_f(c1.x, c2.x), blend_linear_light_f(c1.y, c2.y), blend_linear_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_vivid_light_f(float c1, float c2) {
	return (c1 < 0.5) ? 1.0 - (1.0 - c2) / (2.0 * c1) : c2 / (2.0 * (1.0 - c1));
}
float3 blend_vivid_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_vivid_light_f(c1.x, c2.x), blend_vivid_light_f(c1.y, c2.y), blend_vivid_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_pin_light_f( float c1, float c2) {
	return (2.0 * c1 - 1.0 > c2) ? 2.0 * c1 - 1.0 : ((c1 < 0.5 * c2) ? 2.0 * c1 : c2);
}
float3 blend_pin_light(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_pin_light_f(c1.x, c2.x), blend_pin_light_f(c1.y, c2.y), blend_pin_light_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_hard_lerp_f(float c1, float c2) {
	return floor(c1 + c2);
}
float3 blend_hard_lerp(float2 uv, float3 c1, float3 c2, float opacity) {
		return opacity*tofloat3(blend_hard_lerp_f(c1.x, c2.x), blend_hard_lerp_f(c1.y, c2.y), blend_hard_lerp_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float blend_exclusion_f(float c1, float c2) {
	return c1 + c2 - 2.0 * c1 * c2;
}
float3 blend_exclusion(float2 uv, float3 c1, float3 c2, float opacity) {
	return opacity*tofloat3(blend_exclusion_f(c1.x, c2.x), blend_exclusion_f(c1.y, c2.y), blend_exclusion_f(c1.z, c2.z)) + (1.0-opacity)*c2;
}
float2 scale(float2 uv, float2 center, float2 scale) {
	uv -= center;
	uv /= scale;
	uv += center;
	return uv;
}
static const float p_o190584_cx = 0.000000000;
static const float p_o190584_cy = 0.000000000;
static const float p_o190584_scale_x = 0.700000000;
static const float p_o190584_scale_y = 0.700000000;
static const float p_o190583_amount1 = 0.530000000;
static const float p_o190583_amount2 = 4.370000000;
static const float p_o190548_gradient_0_pos = 0.000000000;
static const float4 p_o190548_gradient_0_col = tofloat4(0.109803997, 0.109803997, 0.145098001, 1.000000000);
static const float p_o190548_gradient_1_pos = 1.000000000;
static const float4 p_o190548_gradient_1_col = tofloat4(0.152941003, 0.152941003, 0.211765006, 1.000000000);
float4 o190548_gradient_gradient_fct(float x) {
  if (x < p_o190548_gradient_0_pos) {
    return p_o190548_gradient_0_col;
  } else if (x < p_o190548_gradient_1_pos) {
    return lerp(p_o190548_gradient_0_col, p_o190548_gradient_1_col, ((x-p_o190548_gradient_0_pos)/(p_o190548_gradient_1_pos-p_o190548_gradient_0_pos)));
  }
  return p_o190548_gradient_1_col;
}
static const float p_o190547_x_scale = 6.000000000;
static const float p_o190547_y_scale = 6.000000000;
static const float p_o190546_amount1 = 4.000000000;
static const float p_o190546_amount2 = 3.930000000;
static const float p_o190529_gradient_0_pos = 0.033889000;
static const float4 p_o190529_gradient_0_col = tofloat4(0.013458000, 0.016340001, 0.035156000, 1.000000000);
static const float p_o190529_gradient_1_pos = 1.000000000;
static const float4 p_o190529_gradient_1_col = tofloat4(0.509276986, 0.746425986, 0.875000000, 1.000000000);
float4 o190529_gradient_gradient_fct(float x) {
  if (x < p_o190529_gradient_0_pos) {
    return p_o190529_gradient_0_col;
  } else if (x < p_o190529_gradient_1_pos) {
    return lerp(p_o190529_gradient_0_col, p_o190529_gradient_1_col, ((x-p_o190529_gradient_0_pos)/(p_o190529_gradient_1_pos-p_o190529_gradient_0_pos)));
  }
  return p_o190529_gradient_1_col;
}
static const float p_o190525_default_in1 = 0.000000000;
static const float p_o190525_default_in2 = 0.000000000;
static const float p_o190582_default_in1 = 0.000000000;
static const float p_o190582_default_in2 = 0.000000000;
static const float p_o190523_default_in1 = 0.000000000;
static const float p_o190523_default_in2 = 0.000000000;
static const float p_o190527_steps = 2.000000000;
static const float p_o190517_value = 0.100000000;
static const float p_o190517_width = 0.360000000;
static const float p_o190516_r = 0.850000000;
static const float p_o190516_a = 1.000000000;
static const float p_o190516_cx = 0.000000000;
static const float p_o190516_cy = 0.000000000;
static const float p_o190514_sx = 10.000000000;
static const float p_o190514_sy = 6.000000000;
static const float p_o190567_default_in1 = 0.000000000;
static const float p_o190567_default_in2 = 3.630000000;
static const float p_o190518_default_in1 = 0.000000000;
static const float p_o190518_default_in2 = 0.000000000;
static const float p_o190519_default_in1 = 0.000000000;
static const float p_o190521_default_in1 = 0.000000000;
static const float p_o190521_default_in2 = 0.000000000;
static const float p_o190522_default_in1 = 0.000000000;
static const float p_o190522_default_in2 = 0.090000000;
static const float p_o190580_translate_x = 10.000000000;
static const float p_o190580_translate_y = 0.000000000;
static const float seed_o190520 = 0.927218318;
static const float p_o190520_edgecolor = 1.000000000;
static const float p_o190578_default_in1 = 0.000000000;
static const float p_o190578_default_in2 = 0.000000000;
static const float p_o190576_value = 0.170000000;
static const float p_o190576_width = 0.050000000;
static const float p_o190576_contrast = 0.140000000;
static const float p_o190577_default_in1 = 0.000000000;
static const float p_o190577_default_in2 = 2.200000000;
static const float p_o190524_value = 0.042700000;
static const float p_o190524_width = 0.015600000;
static const float p_o190579_translate_x = 0.007850000;
static const float p_o190579_translate_y = -0.005000000;
static const float p_o190542_count = 10.000000000;
static const float p_o190542_width = 2.000000000;
float4 o190542_input_in(float2 uv, float _seed_variation_) {
float2 o190516_0_co = tofloat2(p_o190516_cx+0.5,p_o190516_cy+0.5);
float o190516_0_f = dot(2.0*((uv) - o190516_0_co),2.0*((uv) - o190516_0_co));float2 o190514_0_scale = tofloat2(p_o190514_sx, p_o190514_sy*1.73205080757);
float2 o190514_0_uv = (o190516_0_co-(o190516_0_co-(uv))/(sqrt(p_o190516_r-o190516_0_f)*max(p_o190516_a,0.0)+1.0))*o190514_0_scale;
float4 o190514_0_center = beehive_center(o190514_0_uv);float o190514_0_1_f = 1.0-2.0*beehive_dist(o190514_0_center.xy);
float2 o190514_2_scale = tofloat2(p_o190514_sx, p_o190514_sy*1.73205080757);
float2 o190514_2_uv = (uv)*o190514_2_scale;
float4 o190514_2_center = beehive_center(o190514_2_uv);float o190514_0_3_f = 1.0-2.0*beehive_dist(o190514_2_center.xy);
float4 o190516_0_1_rgba = clamp(lerp(tofloat4(tofloat3(o190514_0_1_f), 1.0),tofloat4(tofloat3(o190514_0_3_f), 1.0),step(p_o190516_r,o190516_0_f)),0.0,1.0);
float3 o190517_0_false = clamp((o190516_0_1_rgba.rgb-tofloat3(p_o190517_value))/max(0.0001, p_o190517_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o190517_0_true = tofloat3(1.0)-o190517_0_false;float4 o190517_0_1_rgba = tofloat4(o190517_0_false, o190516_0_1_rgba.a);
float4 o190527_0_1_rgba = tofloat4(floor(o190517_0_1_rgba.rgb*p_o190527_steps)/p_o190527_steps, o190517_0_1_rgba.a);
float2 o190514_4_scale = tofloat2(p_o190514_sx, p_o190514_sy*1.73205080757);
float2 o190514_4_uv = (o190516_0_co-(o190516_0_co-(uv))/(sqrt(p_o190516_r-o190516_0_f)*max(p_o190516_a,0.0)+1.0))*o190514_4_scale;
float4 o190514_4_center = beehive_center(o190514_4_uv);float4 o190514_1_5_fill = tofloat4(o190514_4_uv-o190514_4_center.xy-tofloat2(0.5, 0.57735026919), tofloat2(1.0, 1.15470053838))/o190514_4_scale.xyxy;
float2 o190515_0_c = frac(o190514_1_5_fill.xy+0.5*o190514_1_5_fill.zw);float o190515_0_1_f = o190515_0_c.y;
float2 o190514_6_scale = tofloat2(p_o190514_sx, p_o190514_sy*1.73205080757);
float2 o190514_6_uv = ((o190516_0_co-(o190516_0_co-(uv))/(sqrt(p_o190516_r-o190516_0_f)*max(p_o190516_a,0.0)+1.0))-tofloat2(p_o190580_translate_x, p_o190580_translate_y))*o190514_6_scale;
float4 o190514_6_center = beehive_center(o190514_6_uv);float4 o190514_1_7_fill = tofloat4(o190514_6_uv-o190514_6_center.xy-tofloat2(0.5, 0.57735026919), tofloat2(1.0, 1.15470053838))/o190514_6_scale.xyxy;
float4 o190520_0_bb = o190514_1_7_fill;float o190520_0_1_f = lerp(p_o190520_edgecolor, rand(tofloat2(float((seed_o190520+frac(_seed_variation_))), rand(tofloat2(rand(o190520_0_bb.xy), rand(o190520_0_bb.zw))))), step(0.0000001, dot(o190520_0_bb.zw, tofloat2(1.0))));
float4 o190580_0_1_rgba = tofloat4(tofloat3(o190520_0_1_f), 1.0);
float o190522_0_clamp_false = (dot((o190580_0_1_rgba).rgb, tofloat3(1.0))/3.0)*p_o190522_default_in2;
float o190522_0_clamp_true = clamp(o190522_0_clamp_false, 0.0, 1.0);
float o190522_0_2_f = o190522_0_clamp_false;
float o190521_0_clamp_false = o190515_0_1_f-o190522_0_2_f;
float o190521_0_clamp_true = clamp(o190521_0_clamp_false, 0.0, 1.0);
float o190521_0_1_f = o190521_0_clamp_false;
float4 o190514_1_8_fill = tofloat4(o190514_2_uv-o190514_2_center.xy-tofloat2(0.5, 0.57735026919), tofloat2(1.0, 1.15470053838))/o190514_2_scale.xyxy;
float2 o190515_2_c = frac(o190514_1_8_fill.xy+0.5*o190514_1_8_fill.zw);float o190515_0_3_f = o190515_2_c.y;
float2 o190514_9_scale = tofloat2(p_o190514_sx, p_o190514_sy*1.73205080757);
float2 o190514_9_uv = ((uv)-tofloat2(p_o190580_translate_x, p_o190580_translate_y))*o190514_9_scale;
float4 o190514_9_center = beehive_center(o190514_9_uv);float4 o190514_1_10_fill = tofloat4(o190514_9_uv-o190514_9_center.xy-tofloat2(0.5, 0.57735026919), tofloat2(1.0, 1.15470053838))/o190514_9_scale.xyxy;
float4 o190520_2_bb = o190514_1_10_fill;float o190520_0_3_f = lerp(p_o190520_edgecolor, rand(tofloat2(float((seed_o190520+frac(_seed_variation_))), rand(tofloat2(rand(o190520_2_bb.xy), rand(o190520_2_bb.zw))))), step(0.0000001, dot(o190520_2_bb.zw, tofloat2(1.0))));
float4 o190580_0_3_rgba = tofloat4(tofloat3(o190520_0_3_f), 1.0);
float o190522_3_clamp_false = (dot((o190580_0_3_rgba).rgb, tofloat3(1.0))/3.0)*p_o190522_default_in2;
float o190522_3_clamp_true = clamp(o190522_3_clamp_false, 0.0, 1.0);
float o190522_0_5_f = o190522_3_clamp_false;
float o190521_2_clamp_false = o190515_0_3_f-o190522_0_5_f;
float o190521_2_clamp_true = clamp(o190521_2_clamp_false, 0.0, 1.0);
float o190521_0_3_f = o190521_2_clamp_false;
float4 o190516_1_2_rgba = clamp(lerp(tofloat4(tofloat3(o190521_0_1_f), 1.0),tofloat4(tofloat3(o190521_0_3_f), 1.0),step(p_o190516_r,o190516_0_f)),0.0,1.0);
float o190519_0_clamp_false = (dot((o190516_1_2_rgba).rgb, tofloat3(1.0))/3.0)+(_Time.y*.5);
float o190519_0_clamp_true = clamp(o190519_0_clamp_false, 0.0, 1.0);
float o190519_0_2_f = o190519_0_clamp_false;
float o190518_0_clamp_false = frac(o190519_0_2_f);
float o190518_0_clamp_true = clamp(o190518_0_clamp_false, 0.0, 1.0);
float o190518_0_1_f = o190518_0_clamp_false;
float4 o190526_0_1_rgba = tofloat4(tofloat3(1.0)-tofloat4(tofloat3(o190518_0_1_f), 1.0).rgb, tofloat4(tofloat3(o190518_0_1_f), 1.0).a);
float o190567_0_clamp_false = pow((dot((o190526_0_1_rgba).rgb, tofloat3(1.0))/3.0),p_o190567_default_in2);
float o190567_0_clamp_true = clamp(o190567_0_clamp_false, 0.0, 1.0);
float o190567_0_2_f = o190567_0_clamp_false;
float o190523_0_clamp_false = (dot((o190527_0_1_rgba).rgb, tofloat3(1.0))/3.0)*o190567_0_2_f;
float o190523_0_clamp_true = clamp(o190523_0_clamp_false, 0.0, 1.0);
float o190523_0_1_f = o190523_0_clamp_false;
float o190576_0_step = clamp(((dot((o190516_0_1_rgba).rgb, tofloat3(1.0))/3.0) - (p_o190576_value))/max(0.0001, p_o190576_width)+0.5, 0.0, 1.0);
float o190576_0_false = clamp((min(o190576_0_step, 1.0-o190576_0_step) * 2.0) / (1.0 - p_o190576_contrast), 0.0, 1.0);
float o190576_0_true = 1.0-o190576_0_false;float o190576_0_1_f = o190576_0_false;
float o190577_0_clamp_false = pow((dot((o190526_0_1_rgba).rgb, tofloat3(1.0))/3.0),p_o190577_default_in2);
float o190577_0_clamp_true = clamp(o190577_0_clamp_false, 0.0, 1.0);
float o190577_0_2_f = o190577_0_clamp_false;
float o190578_0_clamp_false = o190576_0_1_f*o190577_0_2_f;
float o190578_0_clamp_true = clamp(o190578_0_clamp_false, 0.0, 1.0);
float o190578_0_1_f = o190578_0_clamp_false;
float o190582_0_clamp_false = o190523_0_1_f+o190578_0_1_f;
float o190582_0_clamp_true = clamp(o190582_0_clamp_false, 0.0, 1.0);
float o190582_0_1_f = o190582_0_clamp_false;
float o190516_4_3_f = clamp(p_o190516_r-o190516_0_f,0.0,1.0);
float3 o190524_0_false = clamp((tofloat4(tofloat3(o190516_4_3_f), 1.0).rgb-tofloat3(p_o190524_value))/max(0.0001, p_o190524_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o190524_0_true = tofloat3(1.0)-o190524_0_false;float4 o190524_0_1_rgba = tofloat4(o190524_0_false, tofloat4(tofloat3(o190516_4_3_f), 1.0).a);
float o190525_0_clamp_false = o190582_0_1_f*(dot((o190524_0_1_rgba).rgb, tofloat3(1.0))/3.0);
float o190525_0_clamp_true = clamp(o190525_0_clamp_false, 0.0, 1.0);
float o190525_0_1_f = o190525_0_clamp_false;
float4 o190529_0_1_rgba = o190529_gradient_gradient_fct(o190525_0_1_f);
return o190529_0_1_rgba;
}
float4 supersample_o190542(float2 uv, float size, int count, float width, float _seed_variation_) {
	float4 rv = tofloat4(0.0);
	float2 step_size = tofloat2(width)/size/float(count);
	uv -= tofloat2(0.5)/size;
	for (int x = 0; x < count; ++x) {
		for (int y = 0; y < count; ++y) {
			rv += o190542_input_in(uv+(tofloat2(float(x), float(y))+tofloat2(0.5))*step_size, _seed_variation_);
		}
	}
	return rv/float(count*count);
}static const float4 p_o190563_color = tofloat4(0.619108021, 0.656727016, 1.000000000, 1.000000000);
static const float p_o190565_default_in1 = 0.000000000;
static const float p_o190565_default_in2 = 0.000000000;
static const float p_o190528_default_in1 = 0.000000000;
static const float p_o190528_default_in2 = 0.000000000;
static const float p_o190562_default_in1 = 0.000000000;
static const float p_o190562_default_in2 = 9.670000000;
static const float p_o190566_translate_x = 0.000000000;
static const float p_o190566_rotate = 0.000000000;
static const float p_o190566_scale_x = 1.000000000;
static const float p_o190566_scale_y = 1.000000000;
static const float4 p_o190575_color = tofloat4(0.196078002, 0.631372988, 0.917647004, 1.000000000);
static const float p_o190574_translate_x = 0.012500000;
static const float p_o190574_translate_y = 0.012500000;
static const float p_o190570_count = 6.000000000;
static const float p_o190570_width = 5.000000000;
float4 o190570_input_in(float2 uv, float _seed_variation_) {
float2 o190516_0_co = tofloat2(p_o190516_cx+0.5,p_o190516_cy+0.5);
float o190516_0_f = dot(2.0*((uv) - o190516_0_co),2.0*((uv) - o190516_0_co));float o190516_4_1_f = clamp(p_o190516_r-o190516_0_f,0.0,1.0);
float3 o190524_0_false = clamp((tofloat4(tofloat3(o190516_4_1_f), 1.0).rgb-tofloat3(p_o190524_value))/max(0.0001, p_o190524_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o190524_0_true = tofloat3(1.0)-o190524_0_false;float4 o190524_0_1_rgba = tofloat4(o190524_0_false, tofloat4(tofloat3(o190516_4_1_f), 1.0).a);
float4 o190561_0_1_rgba = tofloat4(tofloat3(1.0)-tofloat4(tofloat3(o190516_4_1_f), 1.0).rgb, tofloat4(tofloat3(o190516_4_1_f), 1.0).a);
float o190562_0_clamp_false = pow((dot((o190561_0_1_rgba).rgb, tofloat3(1.0))/3.0),p_o190562_default_in2);
float o190562_0_clamp_true = clamp(o190562_0_clamp_false, 0.0, 1.0);
float o190562_0_2_f = o190562_0_clamp_false;
float o190528_0_clamp_false = (dot((o190524_0_1_rgba).rgb, tofloat3(1.0))/3.0)*o190562_0_2_f;
float o190528_0_clamp_true = clamp(o190528_0_clamp_false, 0.0, 1.0);
float o190528_0_1_f = o190528_0_clamp_false;
float4 o190568_0 = textureLod(texture_1, (transform2((o190516_0_co-(o190516_0_co-(uv))/(sqrt(p_o190516_r-o190516_0_f)*max(p_o190516_a,0.0)+1.0)), tofloat2(p_o190566_translate_x*(2.0*1.0-1.0), (-_Time.y*.8)*(2.0*1.0-1.0)), p_o190566_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o190566_scale_x*(2.0*1.0-1.0), p_o190566_scale_y*(2.0*1.0-1.0)))), 0.0);
float4 o190566_0_1_rgba = o190568_0;
float4 o190568_1 = textureLod(texture_1, (transform2((uv), tofloat2(p_o190566_translate_x*(2.0*1.0-1.0), (-_Time.y*.8)*(2.0*1.0-1.0)), p_o190566_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o190566_scale_x*(2.0*1.0-1.0), p_o190566_scale_y*(2.0*1.0-1.0)))), 0.0);
float4 o190566_0_4_rgba = o190568_1;
float4 o190516_2_2_rgba = clamp(lerp(o190566_0_1_rgba,o190566_0_4_rgba,step(p_o190516_r,o190516_0_f)),0.0,1.0);
float o190565_0_clamp_false = o190528_0_1_f*(dot((o190516_2_2_rgba).rgb, tofloat3(1.0))/3.0);
float o190565_0_clamp_true = clamp(o190565_0_clamp_false, 0.0, 1.0);
float o190565_0_1_f = o190565_0_clamp_false;
return tofloat4(tofloat3(o190565_0_1_f), 1.0);
}
float4 supersample_o190570(float2 uv, float size, int count, float width, float _seed_variation_) {
	float4 rv = tofloat4(0.0);
	float2 step_size = tofloat2(width)/size/float(count);
	uv -= tofloat2(0.5)/size;
	for (int x = 0; x < count; ++x) {
		for (int y = 0; y < count; ++y) {
			rv += o190570_input_in(uv+(tofloat2(float(x), float(y))+tofloat2(0.5))*step_size, _seed_variation_);
		}
	}
	return rv/float(count*count);
}
		
			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				float _seed_variation_ = 0.0;
				float2 uv = i.uv;
float o190547_0_1_f = lerp_xor(wave_square(p_o190547_x_scale*(scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))).x), wave_square(p_o190547_y_scale*(scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))).y));
float4 o190548_0_1_rgba = o190548_gradient_gradient_fct(o190547_0_1_f);
float2 o190516_0_co = tofloat2(p_o190516_cx+0.5,p_o190516_cy+0.5);
float o190516_0_f = dot(2.0*((scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))) - o190516_0_co),2.0*((scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))) - o190516_0_co));float2 o190514_0_scale = tofloat2(p_o190514_sx, p_o190514_sy*1.73205080757);
float2 o190514_0_uv = (o190516_0_co-(o190516_0_co-(scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))))/(sqrt(p_o190516_r-o190516_0_f)*max(p_o190516_a,0.0)+1.0))*o190514_0_scale;
float4 o190514_0_center = beehive_center(o190514_0_uv);float o190514_0_1_f = 1.0-2.0*beehive_dist(o190514_0_center.xy);
float2 o190514_2_scale = tofloat2(p_o190514_sx, p_o190514_sy*1.73205080757);
float2 o190514_2_uv = (scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y)))*o190514_2_scale;
float4 o190514_2_center = beehive_center(o190514_2_uv);float o190514_0_3_f = 1.0-2.0*beehive_dist(o190514_2_center.xy);
float4 o190516_0_1_rgba = clamp(lerp(tofloat4(tofloat3(o190514_0_1_f), 1.0),tofloat4(tofloat3(o190514_0_3_f), 1.0),step(p_o190516_r,o190516_0_f)),0.0,1.0);
float3 o190517_0_false = clamp((o190516_0_1_rgba.rgb-tofloat3(p_o190517_value))/max(0.0001, p_o190517_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o190517_0_true = tofloat3(1.0)-o190517_0_false;float4 o190517_0_1_rgba = tofloat4(o190517_0_false, o190516_0_1_rgba.a);
float4 o190527_0_1_rgba = tofloat4(floor(o190517_0_1_rgba.rgb*p_o190527_steps)/p_o190527_steps, o190517_0_1_rgba.a);
float2 o190514_4_scale = tofloat2(p_o190514_sx, p_o190514_sy*1.73205080757);
float2 o190514_4_uv = (o190516_0_co-(o190516_0_co-(scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))))/(sqrt(p_o190516_r-o190516_0_f)*max(p_o190516_a,0.0)+1.0))*o190514_4_scale;
float4 o190514_4_center = beehive_center(o190514_4_uv);float4 o190514_1_5_fill = tofloat4(o190514_4_uv-o190514_4_center.xy-tofloat2(0.5, 0.57735026919), tofloat2(1.0, 1.15470053838))/o190514_4_scale.xyxy;
float2 o190515_0_c = frac(o190514_1_5_fill.xy+0.5*o190514_1_5_fill.zw);float o190515_0_1_f = o190515_0_c.y;
float2 o190514_6_scale = tofloat2(p_o190514_sx, p_o190514_sy*1.73205080757);
float2 o190514_6_uv = ((o190516_0_co-(o190516_0_co-(scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))))/(sqrt(p_o190516_r-o190516_0_f)*max(p_o190516_a,0.0)+1.0))-tofloat2(p_o190580_translate_x, p_o190580_translate_y))*o190514_6_scale;
float4 o190514_6_center = beehive_center(o190514_6_uv);float4 o190514_1_7_fill = tofloat4(o190514_6_uv-o190514_6_center.xy-tofloat2(0.5, 0.57735026919), tofloat2(1.0, 1.15470053838))/o190514_6_scale.xyxy;
float4 o190520_0_bb = o190514_1_7_fill;float o190520_0_1_f = lerp(p_o190520_edgecolor, rand(tofloat2(float((seed_o190520+frac(_seed_variation_))), rand(tofloat2(rand(o190520_0_bb.xy), rand(o190520_0_bb.zw))))), step(0.0000001, dot(o190520_0_bb.zw, tofloat2(1.0))));
float4 o190580_0_1_rgba = tofloat4(tofloat3(o190520_0_1_f), 1.0);
float o190522_0_clamp_false = (dot((o190580_0_1_rgba).rgb, tofloat3(1.0))/3.0)*p_o190522_default_in2;
float o190522_0_clamp_true = clamp(o190522_0_clamp_false, 0.0, 1.0);
float o190522_0_2_f = o190522_0_clamp_false;
float o190521_0_clamp_false = o190515_0_1_f-o190522_0_2_f;
float o190521_0_clamp_true = clamp(o190521_0_clamp_false, 0.0, 1.0);
float o190521_0_1_f = o190521_0_clamp_false;
float4 o190514_1_8_fill = tofloat4(o190514_2_uv-o190514_2_center.xy-tofloat2(0.5, 0.57735026919), tofloat2(1.0, 1.15470053838))/o190514_2_scale.xyxy;
float2 o190515_2_c = frac(o190514_1_8_fill.xy+0.5*o190514_1_8_fill.zw);float o190515_0_3_f = o190515_2_c.y;
float2 o190514_9_scale = tofloat2(p_o190514_sx, p_o190514_sy*1.73205080757);
float2 o190514_9_uv = ((scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y)))-tofloat2(p_o190580_translate_x, p_o190580_translate_y))*o190514_9_scale;
float4 o190514_9_center = beehive_center(o190514_9_uv);float4 o190514_1_10_fill = tofloat4(o190514_9_uv-o190514_9_center.xy-tofloat2(0.5, 0.57735026919), tofloat2(1.0, 1.15470053838))/o190514_9_scale.xyxy;
float4 o190520_2_bb = o190514_1_10_fill;float o190520_0_3_f = lerp(p_o190520_edgecolor, rand(tofloat2(float((seed_o190520+frac(_seed_variation_))), rand(tofloat2(rand(o190520_2_bb.xy), rand(o190520_2_bb.zw))))), step(0.0000001, dot(o190520_2_bb.zw, tofloat2(1.0))));
float4 o190580_0_3_rgba = tofloat4(tofloat3(o190520_0_3_f), 1.0);
float o190522_3_clamp_false = (dot((o190580_0_3_rgba).rgb, tofloat3(1.0))/3.0)*p_o190522_default_in2;
float o190522_3_clamp_true = clamp(o190522_3_clamp_false, 0.0, 1.0);
float o190522_0_5_f = o190522_3_clamp_false;
float o190521_2_clamp_false = o190515_0_3_f-o190522_0_5_f;
float o190521_2_clamp_true = clamp(o190521_2_clamp_false, 0.0, 1.0);
float o190521_0_3_f = o190521_2_clamp_false;
float4 o190516_1_2_rgba = clamp(lerp(tofloat4(tofloat3(o190521_0_1_f), 1.0),tofloat4(tofloat3(o190521_0_3_f), 1.0),step(p_o190516_r,o190516_0_f)),0.0,1.0);
float o190519_0_clamp_false = (dot((o190516_1_2_rgba).rgb, tofloat3(1.0))/3.0)+(_Time.y*.5);
float o190519_0_clamp_true = clamp(o190519_0_clamp_false, 0.0, 1.0);
float o190519_0_2_f = o190519_0_clamp_false;
float o190518_0_clamp_false = frac(o190519_0_2_f);
float o190518_0_clamp_true = clamp(o190518_0_clamp_false, 0.0, 1.0);
float o190518_0_1_f = o190518_0_clamp_false;
float4 o190526_0_1_rgba = tofloat4(tofloat3(1.0)-tofloat4(tofloat3(o190518_0_1_f), 1.0).rgb, tofloat4(tofloat3(o190518_0_1_f), 1.0).a);
float o190567_0_clamp_false = pow((dot((o190526_0_1_rgba).rgb, tofloat3(1.0))/3.0),p_o190567_default_in2);
float o190567_0_clamp_true = clamp(o190567_0_clamp_false, 0.0, 1.0);
float o190567_0_2_f = o190567_0_clamp_false;
float o190523_0_clamp_false = (dot((o190527_0_1_rgba).rgb, tofloat3(1.0))/3.0)*o190567_0_2_f;
float o190523_0_clamp_true = clamp(o190523_0_clamp_false, 0.0, 1.0);
float o190523_0_1_f = o190523_0_clamp_false;
float o190576_0_step = clamp(((dot((o190516_0_1_rgba).rgb, tofloat3(1.0))/3.0) - (p_o190576_value))/max(0.0001, p_o190576_width)+0.5, 0.0, 1.0);
float o190576_0_false = clamp((min(o190576_0_step, 1.0-o190576_0_step) * 2.0) / (1.0 - p_o190576_contrast), 0.0, 1.0);
float o190576_0_true = 1.0-o190576_0_false;float o190576_0_1_f = o190576_0_false;
float o190577_0_clamp_false = pow((dot((o190526_0_1_rgba).rgb, tofloat3(1.0))/3.0),p_o190577_default_in2);
float o190577_0_clamp_true = clamp(o190577_0_clamp_false, 0.0, 1.0);
float o190577_0_2_f = o190577_0_clamp_false;
float o190578_0_clamp_false = o190576_0_1_f*o190577_0_2_f;
float o190578_0_clamp_true = clamp(o190578_0_clamp_false, 0.0, 1.0);
float o190578_0_1_f = o190578_0_clamp_false;
float o190582_0_clamp_false = o190523_0_1_f+o190578_0_1_f;
float o190582_0_clamp_true = clamp(o190582_0_clamp_false, 0.0, 1.0);
float o190582_0_1_f = o190582_0_clamp_false;
float o190516_4_3_f = clamp(p_o190516_r-o190516_0_f,0.0,1.0);
float3 o190524_0_false = clamp((tofloat4(tofloat3(o190516_4_3_f), 1.0).rgb-tofloat3(p_o190524_value))/max(0.0001, p_o190524_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o190524_0_true = tofloat3(1.0)-o190524_0_false;float4 o190524_0_1_rgba = tofloat4(o190524_0_false, tofloat4(tofloat3(o190516_4_3_f), 1.0).a);
float o190525_0_clamp_false = o190582_0_1_f*(dot((o190524_0_1_rgba).rgb, tofloat3(1.0))/3.0);
float o190525_0_clamp_true = clamp(o190525_0_clamp_false, 0.0, 1.0);
float o190525_0_1_f = o190525_0_clamp_false;
float4 o190529_0_1_rgba = o190529_gradient_gradient_fct(o190525_0_1_f);
float4 o190542_0_1_rgba = supersample_o190542(((scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y)))-tofloat2(p_o190579_translate_x, p_o190579_translate_y)), 128.000000000, int(p_o190542_count), p_o190542_width, _seed_variation_);
float4 o190579_0_1_rgba = o190542_0_1_rgba;
float4 o190563_0_1_rgba = p_o190563_color;
float4 o190561_0_1_rgba = tofloat4(tofloat3(1.0)-tofloat4(tofloat3(o190516_4_3_f), 1.0).rgb, tofloat4(tofloat3(o190516_4_3_f), 1.0).a);
float o190562_0_clamp_false = pow((dot((o190561_0_1_rgba).rgb, tofloat3(1.0))/3.0),p_o190562_default_in2);
float o190562_0_clamp_true = clamp(o190562_0_clamp_false, 0.0, 1.0);
float o190562_0_2_f = o190562_0_clamp_false;
float o190528_0_clamp_false = (dot((o190524_0_1_rgba).rgb, tofloat3(1.0))/3.0)*o190562_0_2_f;
float o190528_0_clamp_true = clamp(o190528_0_clamp_false, 0.0, 1.0);
float o190528_0_1_f = o190528_0_clamp_false;
float4 o190568_0 = textureLod(texture_1, (transform2((o190516_0_co-(o190516_0_co-(scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))))/(sqrt(p_o190516_r-o190516_0_f)*max(p_o190516_a,0.0)+1.0)), tofloat2(p_o190566_translate_x*(2.0*1.0-1.0), (-_Time.y*.8)*(2.0*1.0-1.0)), p_o190566_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o190566_scale_x*(2.0*1.0-1.0), p_o190566_scale_y*(2.0*1.0-1.0)))), 0.0);
float4 o190566_0_1_rgba = o190568_0;
float4 o190568_1 = textureLod(texture_1, (transform2((scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))), tofloat2(p_o190566_translate_x*(2.0*1.0-1.0), (-_Time.y*.8)*(2.0*1.0-1.0)), p_o190566_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o190566_scale_x*(2.0*1.0-1.0), p_o190566_scale_y*(2.0*1.0-1.0)))), 0.0);
float4 o190566_0_4_rgba = o190568_1;
float4 o190516_2_4_rgba = clamp(lerp(o190566_0_1_rgba,o190566_0_4_rgba,step(p_o190516_r,o190516_0_f)),0.0,1.0);
float o190565_0_clamp_false = o190528_0_1_f*(dot((o190516_2_4_rgba).rgb, tofloat3(1.0))/3.0);
float o190565_0_clamp_true = clamp(o190565_0_clamp_false, 0.0, 1.0);
float o190565_0_1_f = o190565_0_clamp_false;
float4 o190546_0_b = o190529_0_1_rgba;
float4 o190546_0_l;
float o190546_0_a;

o190546_0_l = o190579_0_1_rgba;
o190546_0_a = p_o190546_amount1*1.0;
o190546_0_b = tofloat4(blend_additive((scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))), o190546_0_l.rgb, o190546_0_b.rgb, o190546_0_a*o190546_0_l.a), min(1.0, o190546_0_b.a+o190546_0_a*o190546_0_l.a));

o190546_0_l = o190563_0_1_rgba;
o190546_0_a = p_o190546_amount2*o190565_0_1_f;
o190546_0_b = tofloat4(blend_additive((scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))), o190546_0_l.rgb, o190546_0_b.rgb, o190546_0_a*o190546_0_l.a), min(1.0, o190546_0_b.a+o190546_0_a*o190546_0_l.a));

float4 o190546_0_2_rgba = o190546_0_b;
float4 o190575_0_1_rgba = p_o190575_color;
float4 o190570_0_1_rgba = supersample_o190570(((scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y)))-tofloat2(p_o190574_translate_x, p_o190574_translate_y)), 128.000000000, int(p_o190570_count), p_o190570_width, _seed_variation_);
float4 o190574_0_1_rgba = o190570_0_1_rgba;
float4 o190583_0_b = o190548_0_1_rgba;
float4 o190583_0_l;
float o190583_0_a;

o190583_0_l = o190546_0_2_rgba;
o190583_0_a = p_o190583_amount1*1.0;
o190583_0_b = tofloat4(blend_lighten((scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))), o190583_0_l.rgb, o190583_0_b.rgb, o190583_0_a*o190583_0_l.a), min(1.0, o190583_0_b.a+o190583_0_a*o190583_0_l.a));

o190583_0_l = o190575_0_1_rgba;
o190583_0_a = p_o190583_amount2*(dot((o190574_0_1_rgba).rgb, tofloat3(1.0))/3.0);
o190583_0_b = tofloat4(blend_additive((scale((uv), tofloat2(0.5+p_o190584_cx, 0.5+p_o190584_cy), tofloat2(p_o190584_scale_x, p_o190584_scale_y))), o190583_0_l.rgb, o190583_0_b.rgb, o190583_0_a*o190583_0_l.a), min(1.0, o190583_0_b.a+o190583_0_a*o190583_0_l.a));

float4 o190583_0_2_rgba = o190583_0_b;
float4 o190584_0_1_rgba = o190583_0_2_rgba;

				// sample the generated texture
				fixed4 col = o190584_0_1_rgba;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}



