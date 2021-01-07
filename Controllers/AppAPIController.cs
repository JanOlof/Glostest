using Glostest.Models;
using Glostest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Glostest.Controllers
{
    //http://www.c-sharpcorner.com/article/enable-cors-in-asp-net-webapi-2/
    //Install-Package Microsoft.AspNet.WebApi.Cors
    [EnableCors(origins: "*", headers: "*", methods: "*")] //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class AppAPIController : ApiController
    {
        private WordModel db = new WordModel();
        [Route("api/wordtest")]
        public List<WordPairComplexDTO> GetWordtest(int wordGroupId)
        {
            List<WordPairComplexDTO> wordList = new List<WordPairComplexDTO>();
            SynonymsView viewModel = new SynonymsView();
            viewModel.FillViewModel(wordGroupId);

            string languageCode1 = "";
            string languageCode2 = "";

            foreach (var viewModelSynonym in viewModel.SortedSynonyms)
            {
                WordPairComplexDTO synonym = new WordPairComplexDTO { Id = viewModelSynonym.Id };
                wordList.Add(synonym);
                foreach (var list in viewModelSynonym.SortedWordList)
                {
                    //Första ordets språk avgör för listan som bara har två språk. Språken är sorterade i bokstavsordning. 
                    //Tveksam lösning, men gör appen enklare och funkar väl ok.. Det kan finnas mer än och mindre än två språk vilket inte appen hanterar så väl 
                    if (languageCode1 == "")
                        languageCode1 = list.Value.Language.Code;
                    else if (languageCode2 == "" && list.Value.Language.Code != languageCode1)
                        languageCode2 = list.Value.Language.Code;

                if (list.Value.Language.Code == languageCode1)
                    {
                        foreach (var word in list.Value.Words)
                        {
                            synonym.Word1.Add(new WordDTO { Id = word.Id, Language = word.Language.Name, LanguageId = word.LanguageId, Text = word.Text });
                        }
                    }
                    else if (list.Value.Language.Code == languageCode2)
                    {
                        foreach (var word in list.Value.Words)
                        {
                            synonym.Word2.Add(new WordDTO { Id = word.Id, Language = word.Language.Name, LanguageId = word.LanguageId, Text = word.Text });
                        }
                    }
                }
            }
            return wordList;
        }

        //Gamla för webbapp. Ska bort
        [Route("api/wordtestcomplex")]
        public List<WordPairComplexDTO> GetWordtestComplex(int wordGroupId, string languageCode1, string languageCode2)
        {
            List<WordPairComplexDTO> wordList = new List<WordPairComplexDTO>();
            SynonymsView viewModel = new SynonymsView();
            viewModel.FillViewModel(wordGroupId);

            foreach (var viewModelSynonym in viewModel.SortedSynonyms)
            {
                if (ShouldBeIncluded(viewModelSynonym, languageCode1, languageCode2))
                {
                    WordPairComplexDTO synonym = new WordPairComplexDTO { Id = viewModelSynonym.Id };
                    wordList.Add(synonym);
                    foreach (var list in viewModelSynonym.SortedWordList)
                    {
                        if (list.Value.Language.Code == languageCode1)
                        {
                            foreach (var word in list.Value.Words)
                            {
                                synonym.Word1.Add(new WordDTO { Id = word.Id, Language = word.Language.Name, LanguageId = word.LanguageId, Text = word.Text });
                            }
                        }
                        else if (list.Value.Language.Code == languageCode2)
                        {
                            foreach (var word in list.Value.Words)
                            {
                                synonym.Word2.Add(new WordDTO { Id = word.Id, Language = word.Language.Name, LanguageId = word.LanguageId, Text = word.Text });
                            }
                        }
                    }
                }
            }
             return wordList;
        }

        [Route("api/wordgroups")]
        public List<WordGroupDTO> GetWordGroups()
        {
            var dbWordGroupList = db.WordGroup;
            List<WordGroupDTO>wordGroupDTOList = new List<WordGroupDTO>();
            foreach (var item in dbWordGroupList.OrderBy(d => d.Description))
            {
                wordGroupDTOList.Add(new WordGroupDTO { Id = item.Id, Description = item.Description });
            }
            return wordGroupDTOList;
        }
        [Route("api/wordgroupsbyuser")]
        public List<WordGroupDTO> GetWordGroupsByUser(int userId)
        {
            var dbWordGroupList = db.WordGroup.Where(g => g.UserId == userId);
            List<WordGroupDTO> wordGroupDTOList = new List<WordGroupDTO>();
            foreach (var item in dbWordGroupList.OrderBy(d => d.Description))
            {
                wordGroupDTOList.Add(new WordGroupDTO { Id = item.Id, Description = item.Description });
            }
            return wordGroupDTOList;
        }

        [Route("api/users")]
        public List<UserDTO> GetUsers()
        {
            var users = db.User.Select(user => new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
            }).ToList();
            return users;
        }
        //Vi behöver ha ord på varje språk som vi ska göra prov på
        private bool ShouldBeIncluded(SortedSynonym synonym, string languageCode1, string languageCode2)
        {
            bool languageCode1Exists = false;
            bool languageCode2Exists = false;
            foreach (var wordList in synonym.SortedWordList)
            {
                if (wordList.Value.Language.Code == languageCode1)
                    languageCode1Exists = true;
                else if (wordList.Value.Language.Code == languageCode2)
                    languageCode2Exists = true;
            }
            if (languageCode1Exists && languageCode2Exists)
                return true;
            else
                return false;
        }
        //Ta bort blankspace på slutet av varje ord
        private void CleanForPresentation(List<WordPairDTO> wordList) 
        {
            foreach (var wordPair in wordList)
            {
                wordPair.Word1 = wordPair.Word1.Trim();
                wordPair.Word2 = wordPair.Word2.Trim();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
