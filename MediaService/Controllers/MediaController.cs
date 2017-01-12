using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MediaService.Models;

namespace MediaService.Controllers
{
    public class MediaController : ApiController
    {
        private MediaServiceContext db = new MediaServiceContext();
        private string tempString = "test";
        // GET: api/Media
        public IQueryable<MediaDTO> GetMedia()
        {
            //return db.Media;
            var media = from m in db.Media
                        select new MediaDTO()
                        {
                            Id = m.Id,
                            Title = m.Title,
                            AuthorName = m.Author.Name
                        };
            return media;

        }

        // GET: api/Media/5
        [ResponseType(typeof(MediaDetailDTO))]
        public async Task<IHttpActionResult> GetMedia(int id)
        {
            var media = await db.Media.Include(m => m.Author).Select(m =>
                new MediaDetailDTO()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Price = m.Price,
                    AuthorName = m.Author.Name,
                    Genre = m.Genre

                }).SingleOrDefaultAsync(m => m.Id == id);

            //Media media = await db.Media.FindAsync(id);

            if (media == null)
            {
                return NotFound();
            }

            return Ok(media);
        }

        // PUT: api/Media/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMedia(int id, Media media)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != media.Id)
            {
                return BadRequest();
            }

            db.Entry(media).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Media
        [ResponseType(typeof(Media))]
        public async Task<IHttpActionResult> PostMedia(Media media)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Media.Add(media);
            await db.SaveChangesAsync();

            db.Entry(media).Reference(m => m.Author).Load();

            var dto = new MediaDTO()
            {
                Id = media.Id,
                Title = media.Title,
                AuthorName = media.Author.Name
            };           

            return CreatedAtRoute("DefaultApi", new { id = media.Id }, dto);
        }

        // DELETE: api/Media/5
        [ResponseType(typeof(Media))]
        public async Task<IHttpActionResult> DeleteMedia(int id)
        {
            Media media = await db.Media.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }

            db.Media.Remove(media);
            await db.SaveChangesAsync();

            return Ok(media);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MediaExists(int id)
        {
            return db.Media.Count(e => e.Id == id) > 0;
        }
    }
}