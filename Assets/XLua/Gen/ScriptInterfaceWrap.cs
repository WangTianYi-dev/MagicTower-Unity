#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class ScriptInterfaceWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ScriptInterface);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 18, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Log", _m_Log_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetGlobal", _m_SetGlobal_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetGlobal", _m_GetGlobal_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "VerifyGlobal", _m_VerifyGlobal_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateUnitEntity", _m_CreateUnitEntity_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateGroundEntity", _m_CreateGroundEntity_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReplaceUnitEntity", _m_ReplaceUnitEntity_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReplaceGroundEntity", _m_ReplaceGroundEntity_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "KillUnitEntity", _m_KillUnitEntity_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "KillGroundEntity", _m_KillGroundEntity_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddSetting", _m_AddSetting_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUnitEntityName", _m_GetUnitEntityName_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetGroundEntityName", _m_GetGroundEntityName_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PlayerSuspend", _m_PlayerSuspend_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PlayerResume", _m_PlayerResume_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddContinuousEvent", _m_AddContinuousEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddEvent", _m_AddEvent_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "ScriptInterface does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Log_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _message = LuaAPI.lua_tostring(L, 1);
                    
                    ScriptInterface.Log( _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetGlobal_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    string _value = LuaAPI.lua_tostring(L, 2);
                    
                    ScriptInterface.SetGlobal( _name, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGlobal_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = ScriptInterface.GetGlobal( _name );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_VerifyGlobal_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    string _value = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = ScriptInterface.VerifyGlobal( _name, _value );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateUnitEntity_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = ScriptInterface.CreateUnitEntity( _name, _x, _y );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateGroundEntity_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = ScriptInterface.CreateGroundEntity( _name, _x, _y );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReplaceUnitEntity_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = ScriptInterface.ReplaceUnitEntity( _name, _x, _y );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReplaceGroundEntity_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = ScriptInterface.ReplaceGroundEntity( _name, _x, _y );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_KillUnitEntity_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 1);
                    int _y = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = ScriptInterface.KillUnitEntity( _x, _y );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_KillGroundEntity_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 1);
                    int _y = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = ScriptInterface.KillGroundEntity( _x, _y );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddSetting_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    string _value = LuaAPI.lua_tostring(L, 2);
                    int _x = LuaAPI.xlua_tointeger(L, 3);
                    int _y = LuaAPI.xlua_tointeger(L, 4);
                    
                    ScriptInterface.AddSetting( _key, _value, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnitEntityName_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 1);
                    int _y = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = ScriptInterface.GetUnitEntityName( _x, _y );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGroundEntityName_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 1);
                    int _y = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = ScriptInterface.GetGroundEntityName( _x, _y );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayerSuspend_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    ScriptInterface.PlayerSuspend(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayerResume_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    ScriptInterface.PlayerResume(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddContinuousEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaFunction _func = (XLua.LuaFunction)translator.GetObject(L, 1, typeof(XLua.LuaFunction));
                    float _time = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    ScriptInterface.AddContinuousEvent( _func, _time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaFunction _func = (XLua.LuaFunction)translator.GetObject(L, 1, typeof(XLua.LuaFunction));
                    
                    ScriptInterface.AddEvent( _func );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
