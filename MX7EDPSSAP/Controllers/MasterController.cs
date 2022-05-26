using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MX7EDPSSAP.Application.Constants;
using MX7EDPSSAP.Helpers;
using MX7EDPSSAP.Infrastructure;
using MX7EDPSSAP.Model;
using MX7EDPSSAP.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MX7EDPSSAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly ILogger<MasterController> _logger;
        private readonly IMasterSvc _masterSvc;
        public MasterController(ILogger<MasterController> logger, IMasterSvc masterSvc)
        {
            _logger = logger;
            _masterSvc = masterSvc;
        }
        [HttpGet]
        [Route("{mdType}")]
        public async Task<IActionResult> GetAislesList(string mdType)
        {
            try
            {
                if (!Enum.TryParse(mdType, true, out MasterDataEnum.MasterDataTableName masterDataType))
                {
                    throw new InvalidParametersException($"Invalid Master Data Type: {mdType}.");
                }

                List<dynamic> results = await _masterSvc.GetMasterDataRecordAsync(masterDataType);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, CommonHelper.GenerateStandardErrorResponse(ex, (int)HttpStatusCode.InternalServerError, "Error retrieving aisle records list."));
            }
        }

        [HttpGet]
        [Route("ImportRawData")]
        public async Task<IActionResult> ImportData()
        {
            try
            {
                List<dynamic> results = await _masterSvc.retreiveImport();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, CommonHelper.GenerateStandardErrorResponse(ex, (int)HttpStatusCode.InternalServerError, "Error retrieving aisle records list."));
            }
        }

        [HttpGet]
        [Route("ImportSOHData")]
        public async Task<IActionResult> ImportSOHData()
        {
            try
            {
                List<dynamic> results = await _masterSvc.retreiveSOHImport();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, CommonHelper.GenerateStandardErrorResponse(ex, (int)HttpStatusCode.InternalServerError, "Error retrieving aisle records list."));
            }
        }

        [HttpGet]
        [Route("exportPutData")]
        public async Task<IActionResult> ExportPutData(string store_code)
        {
            try
            {
                if (store_code== "" && store_code == null)
                {
                    throw new InvalidParametersException($"Invalid Master Data Type: {store_code}.");
                }
                List<dynamic> results = await _masterSvc.exportPutData(store_code);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, CommonHelper.GenerateStandardErrorResponse(ex, (int)HttpStatusCode.InternalServerError, "Error retrieving aisle records list."));
            }
        }

        [HttpGet]
        [Route("exportPutDetailData")]
        public async Task<IActionResult> ExportPutDetailData(string store_code)
        {
            try
            {
                if (store_code=="" && store_code ==null)
                {
                    throw new InvalidParametersException($"Invalid Master Data Type: {store_code}.");
                }
                List<dynamic> results = await _masterSvc.exportPutDetailData(store_code);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, CommonHelper.GenerateStandardErrorResponse(ex, (int)HttpStatusCode.InternalServerError, "Error retrieving aisle records list."));
            }
        }



    }
}
