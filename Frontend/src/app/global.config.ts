import { CacheLocation } from "@auth0/auth0-spa-js";

export class GlobalConstants {
	public static AuthDomain = "dev-nhscnbma.us.auth0.com"
	public static AuthClientID = "NXdhsjgVeHNxeanjqGteooj6EyyGRCqi";
	public static API_URL : string = 'http://localhost:8080/api{0}';
	public static AuthRefreshToken : boolean = true;
  	public static AuthCacheLocation : CacheLocation = 'localstorage';
}	