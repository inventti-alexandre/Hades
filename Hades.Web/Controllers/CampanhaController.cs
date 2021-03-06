﻿using Hades.Application.Interface;
using Hades.Domain.Entities;
using Hades.Web.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hades.Web.Controllers
{
    public class CampanhaController : BaseController
    {
        private readonly ICampanhaAppService _campanhaAppService;
        private readonly ICampanhaParticipanteAppService _campanhaParticipanteAppService;
        private readonly IUsuarioAppService _usuarioAppService;

        public CampanhaController(ICampanhaAppService campanhaAppService, ICampanhaParticipanteAppService campanhaParticipanteAppService, IUsuarioAppService usuarioAppService)
        {
            _campanhaAppService = campanhaAppService;
            _campanhaParticipanteAppService = campanhaParticipanteAppService;
            _usuarioAppService = usuarioAppService;
        }

        public ActionResult Index()
        {
            ViewBag.NomeUsuario = Session["Nome"].ToString();
            return View();
        }

        public ActionResult BuscaGridCampanhas()
        {
            var campanhaViewModel = _campanhaAppService.GetCampanhas((int)Session["IdUsuario"]);
            if (!campanhaViewModel.IsSuccessStatusCode)
                return ErrorMessage("Erro ao trazer campanhas");
            var campanha = JsonConvert.DeserializeObject<IEnumerable<CampanhaViewModel>>(campanhaViewModel.Content.ReadAsStringAsync().Result);
            return View("_TabelaCampanha", campanha);
        }

        public ActionResult BuscaGridTodasCampanhas()
        {
            var campanhaViewModel = _campanhaAppService.GetTodasCampanhas((int)Session["IdUsuario"]);
            if (!campanhaViewModel.IsSuccessStatusCode)
                return ErrorMessage("Erro ao trazer campanhas");
            var campanha = JsonConvert.DeserializeObject<IEnumerable<CampanhaViewModel>>(campanhaViewModel.Content.ReadAsStringAsync().Result);
            return View("_TabelaCampanha", campanha);
        }

        public ActionResult Details(int idCampanha)
        {
            var campanha = _campanhaAppService.GetCampanha(idCampanha);
            if (!campanha.IsSuccessStatusCode)
                return ErrorMessage("Erro ao trazer Campanha");
            var mostraCampanha =
                JsonConvert.DeserializeObject<CampanhaViewModel>(campanha.Content.ReadAsStringAsync().Result);

            return View(mostraCampanha);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateConfirmed(CampanhaDto campanha)
        {
            var response = _campanhaAppService.Post(campanha);
            return response.IsSuccessStatusCode ? Json("OK") : ErrorMessage("Erro ao criar Campanha");
        }

        public ActionResult Edit(int idCampanha)
        {
            var response = _campanhaAppService.GetCampanha(idCampanha);
            if (!response.IsSuccessStatusCode)
                return ErrorMessage("Erro ao trazer campanha");
            var campanha = JsonConvert.DeserializeObject<CampanhaViewModel>(response.Content.ReadAsStringAsync().Result);

            return View(campanha);
        }

        public ActionResult EditConfirmed(CampanhaDto campanha)
        {
            var response = _campanhaAppService.Put(campanha);
            return response.IsSuccessStatusCode ? Json("OK") : ErrorMessage("Erro ao editar campanha");
        }

        public ActionResult Delete(int idCampanha)
        {
            var retorno = _campanhaAppService.Delete(idCampanha);
            return !retorno.IsSuccessStatusCode ? ErrorMessage("Erro ao deletar campanha") : Json("OK");
        }
    }
}