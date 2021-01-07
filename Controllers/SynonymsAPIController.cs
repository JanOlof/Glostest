using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Glostest;
using Glostest.Models;
using Glostest.ViewModels;

namespace Glostest.Controllers
{
    public class SynonymsAPIController : ApiController
    {
        //private WordModel db = new WordModel();

        //// GET: api/SynonymsAPI
        //public List<SynonymsDTO> GetSynonyms()
        //{
        //    List<SynonymsDTO> synonymsList = new List<SynonymsDTO>();
        //    SynonymsView viewModel = new SynonymsView(); 
        //    viewModel.FillViewModel(1); //ToDo ta bort eller fundera om controllern ska vara kvar

        //    foreach (var viewModelSynonym in viewModel.SortedSynonyms)
        //    {
        //        SynonymsDTO synonym = new SynonymsDTO {Id = viewModelSynonym.Id};
        //        synonymsList.Add(synonym);
        //        foreach (var list in viewModelSynonym.SortedWordList)
        //        {
        //            WordListByLanguageDTO wordList = new WordListByLanguageDTO { Language = list.Value.Language.Name };
        //            foreach (var word in list.Value.Words)
        //            {
        //                WordDTO wordDTO = new WordDTO { Id = word.Id, Text = word.Text, Language = word.Language.Name, LanguageId = word.LanguageId };
        //                wordList.Words.Add(wordDTO);
        //            }
        //            synonym.WordList.Add(wordList);
        //        }
        //    }
        //    return synonymsList;
        //}

        //// GET: api/SynonymsAPI/5
        //[ResponseType(typeof(Synonyms))]
        //public IHttpActionResult GetSynonyms(int id)
        //{
        //    Synonyms synonyms = db.Synonyms.Find(id);
        //    if (synonyms == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(synonyms);
        //}

        //// PUT: api/SynonymsAPI/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutSynonyms(int id, Synonyms synonyms)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != synonyms.SynonymId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(synonyms).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SynonymsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/SynonymsAPI
        //[ResponseType(typeof(Synonyms))]
        //public IHttpActionResult PostSynonyms(Synonyms synonyms)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Synonyms.Add(synonyms);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (SynonymsExists(synonyms.SynonymId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = synonyms.SynonymId }, synonyms);
        //}

        //// DELETE: api/SynonymsAPI/5
        //[ResponseType(typeof(Synonyms))]
        //public IHttpActionResult DeleteSynonyms(int id)
        //{
        //    Synonyms synonyms = db.Synonyms.Find(id);
        //    if (synonyms == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Synonyms.Remove(synonyms);
        //    db.SaveChanges();

        //    return Ok(synonyms);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool SynonymsExists(int id)
        //{
        //    return db.Synonyms.Count(e => e.SynonymId == id) > 0;
        //}
    }
}