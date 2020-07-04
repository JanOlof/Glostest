using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Glostest;

namespace Glostest.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WordsAPIController : ApiController
    {
        private WordModel db = new WordModel();

        // GET: api/WordsAPI
        public IQueryable<Word> GetWord()
        {
            return db.Word;
        }

        // GET: api/WordsAPI/5
        [ResponseType(typeof(Word))]
        public IHttpActionResult GetWord(int id)
        {
            Word word = db.Word.Find(id);
            if (word == null)
            {
                return NotFound();
            }

            return Ok(word);
        }

        // PUT: api/WordsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWord(int id, Word word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != word.Id)
            {
                return BadRequest();
            }

            db.Entry(word).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WordExists(id))
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

        // POST: api/WordsAPI
        [ResponseType(typeof(Word))]
        public IHttpActionResult PostWord(Word word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Word.Add(word);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = word.Id }, word);
        }

        // DELETE: api/WordsAPI/5
        [ResponseType(typeof(Word))]
        public IHttpActionResult DeleteWord(int id)
        {
            Word word = db.Word.Find(id);
            if (word == null)
            {
                return NotFound();
            }

            db.Word.Remove(word);
            db.SaveChanges();

            return Ok(word);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WordExists(int id)
        {
            return db.Word.Count(e => e.Id == id) > 0;
        }
    }
}