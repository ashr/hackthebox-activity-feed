using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace hackthebox_activity_feed{
    public class ActivityModel{
        /*
<ul class="DataList ActivityComments Hidden"></ul></li>
<li id="Activity_1243038" class="Item Activity Activity-PictureChange HasPhoto">
<div class="Author Photo"><a href="/profile/r00t44" class="PhotoWrap"><img src="https://forum.hackthebox.eu/uploads/userpics/842/nIU75GIVUUWXM.jpeg" class="ProfilePhoto ProfilePhotoMedium" aria-hidden="true" /></a></div>
<div class="ItemContent Activity">
<div class="Title" role="heading" aria-level="3"><a rel="nofollow" href="/profile/r00t44">r00t44</a> changed <a rel="nofollow" href="/profile/r00t44">their</a> profile picture.</div> <div class="Excerpt userContent"><img src="https://forum.hackthebox.eu/uploads/userpics/842/nIU75GIVUUWXM.jpeg" alt="Thumbnail" /></div> <div class="Meta">
<span class="MItem DateCreated">December 19</span>
</div>
</div>
      */
      public string activityId;
      public string authorName;
      public string authorPicUrl;
      public string content;
      public string postDate;
      public string activityHash;
    }
}