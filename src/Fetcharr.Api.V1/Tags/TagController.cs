using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NzbDrone.Core.Datastore.Events;
using NzbDrone.Core.Messaging.Events;
using NzbDrone.Core.Tags;
using NzbDrone.Http.REST.Attributes;
using NzbDrone.SignalR;
using Fetcharr.Http;
using Fetcharr.Http.REST;

namespace Fetcharr.Api.V1.Tags
{
    [V1ApiController]
    public class TagController : RestControllerWithSignalR<TagResource, Tag>, IHandle<TagsUpdatedEvent>
    {
        private readonly ITagService _tagService;

        public TagController(IBroadcastSignalRMessage signalRBroadcaster,
                         ITagService tagService)
            : base(signalRBroadcaster)
        {
            _tagService = tagService;
        }

        public override TagResource GetResourceById(int id)
        {
            return _tagService.GetTag(id).ToResource();
        }

        [HttpGet]
        [Produces("application/json")]
        public List<TagResource> GetAll()
        {
            return _tagService.All().ToResource();
        }

        [RestPostById]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<TagResource> Create(TagResource resource)
        {
            return Created(_tagService.Add(resource.ToModel()).Id);
        }

        [RestPutById]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<TagResource> Update(TagResource resource)
        {
            _tagService.Update(resource.ToModel());
            return Accepted(resource.Id);
        }

        [RestDeleteById]
        public void DeleteTag(int id)
        {
            _tagService.Delete(id);
        }

        [NonAction]
        public void Handle(TagsUpdatedEvent message)
        {
            BroadcastResourceChange(ModelAction.Sync);
        }
    }
}
