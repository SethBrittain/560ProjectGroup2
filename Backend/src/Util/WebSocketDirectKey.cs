namespace Pidgin.Util;

public class WebSocketDirectKey(int uid1, int uid2)
{
    public int uid1 = uid1, uid2 = uid2;

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (GetType() != obj.GetType())
            return false;

        WebSocketDirectKey key = (WebSocketDirectKey)obj;
        return (uid1 == key.uid1 && uid2 == key.uid2) || (uid1 == key.uid2 && uid2 == key.uid1);
    }

    public override int GetHashCode()
    {
        return uid1.GetHashCode() ^ uid2.GetHashCode();
    }
}
