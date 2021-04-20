using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using GreenFlux.Api.Models.RequestModels;
using GreenFlux.Service;

namespace GreenFlux.Api.Controllers
{
    [Route(Routes.GroupsPath)]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            this._groupService = groupService;
        }

        /// <summary>
        /// Action to get all groups
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            var groups = _groupService.GetAll();
            if (groups?.Count() > 0)
            {
                //TODO: Paging
                return Ok(groups);
            }

            return NotFound();
        }

        /// <summary>
        /// Action to get specific group by id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet(Routes.GroupId)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGroupDetail(Guid groupId)
        {
            var group = await _groupService.GetByIdAsync(groupId);
            if (group != null)
            {
                return Ok(group);
            }

            return NotFound();
        }

        /// <summary>
        /// Action to create new group
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] CreateGroup model)
        {
            var group = _groupService.Create(model.Name, model.Capacity);
            if (group != null)
            {
                //TODO: Dynamic Created Link
                return Created(new Uri($"http://localhost:40356/api/groups/{group.Id}"), group);
            }

            return BadRequest();
        }

        /// <summary>
        /// Action to update specific group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateGroup"></param>
        /// <returns></returns>
        [HttpPut(Routes.GroupId)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateGroup updateGroup)
        {
            var updatedGroup = await _groupService.Update(id, updateGroup);
            return Ok(updatedGroup);
        }

        /// <summary>
        /// Action to get delete specific group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Routes.GroupId)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(Guid id)
        {
            _groupService.Remove(id);
            return NoContent();
        }
    }
}
