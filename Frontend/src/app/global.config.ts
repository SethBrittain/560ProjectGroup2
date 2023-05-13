import { CacheLocation } from "@auth0/auth0-spa-js";

export class GlobalConstants {
	public static AuthDomain = "dev-nhscnbma.us.auth0.com"
	public static AuthClientID = "n5pO2NJrhHtNi8UzbzeI0HKFh6d3UbMu";
	public static API_URL : string = 'https://pidgin.sethbrittain.dev/api{0}';
	public static AuthRefreshToken : boolean = true;
  	public static AuthCacheLocation : CacheLocation = 'localstorage';
}	
