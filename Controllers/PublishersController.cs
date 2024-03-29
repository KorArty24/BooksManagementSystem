﻿using AspNetCorePublisherWebAPI.Models;
using AspNetCorePublisherWebAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCorePublisherWebAPI.Controllers
{
    [Route("api/publishers")]
    public class PublishersController : Controller
    {
        IBookstoreRepository _rep;
        public PublishersController(IBookstoreRepository rep)
        {
            _rep = rep;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_rep.GetPublishers());
        }
        

        [HttpGet("{id}", Name = "GetPublisher")]
        public IActionResult Get(int id, bool includeBooks = false)

        {
            var publisher = _rep.GetPublisher(id, includeBooks);
            if (publisher == null) return NotFound();
            return Ok(publisher);
        }
        [HttpPost]
        public IActionResult Post([FromBody] PublisherCreateDTO publisher)
        {
            if (publisher == null) return BadRequest();
            if (publisher.Established < 1534)
                ModelState.AddModelError("Established",
"The first publishing house was founded in 1534.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var publisherToAdd = new PublisherDTO
            {
                Established = publisher.Established,
                Name = publisher.Name

            };
            _rep.AddPublisher(publisherToAdd);
            _rep.Save();
            return CreatedAtRoute("GetPublisher", new
            {
                id = publisherToAdd.Id
            }, publisherToAdd);
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]PublisherUpdateDTO publisher)
        {
            if (publisher == null) return BadRequest();
            if (publisher.Established < 1534)
                ModelState.AddModelError("Established",
                "The oldest publishing house was founded in 1534.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var publisherExists = _rep.PublisherExists(id);
            if (!publisherExists) return NotFound();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public IActionResult Patch(int id,[FromBody]JsonPatchDocument<PublisherUpdateDTO> publisher  )
        {
            if (publisher == null) return BadRequest();
            var publisherToUpdate = _rep.GetPublisher(id);
            if (publisherToUpdate == null) return NotFound();
            var publisherPatch = new PublisherUpdateDTO()
            {
                Name = publisherToUpdate.Name,
                Established = publisherToUpdate.Established
            };
            publisher.ApplyTo(publisherPatch, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _rep.UpdatePublisher(id, publisherPatch);
            _rep.Save();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var publisherToDelete = _rep.GetPublisher(id);
            if (publisherToDelete == null) return NotFound();
            _rep.DeletePublisher(publisherToDelete);
            _rep.Save();
            return NoContent();
        }
    }
}
