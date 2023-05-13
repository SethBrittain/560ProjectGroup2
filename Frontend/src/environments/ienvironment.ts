import { CacheLocation } from "@auth0/auth0-spa-js";

export interface IEnvironment {
	AuthDomain : string,
	AuthClientID : string,
	ApiUrl : string,
	AuthRefreshToken : boolean,
	AuthCacheLocation : CacheLocation,
	LoggedInUrl : string
}