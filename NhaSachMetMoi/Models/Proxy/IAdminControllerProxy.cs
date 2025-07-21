using System.Web.Mvc;

public interface IAdminControllerProxy
{
    ActionResult IndexAd(string sortOrder, int? page);
    ActionResult IndexDH(string sortOrder, int? page);
    ActionResult IndexB(string sortOrder, int? page);
    ActionResult IndexKH( int? page);



}
