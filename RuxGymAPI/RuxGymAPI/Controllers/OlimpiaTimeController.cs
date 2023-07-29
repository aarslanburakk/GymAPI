using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Newtonsoft.Json;
using RuxGymAPI.Context;
using RuxGymAPI.Models;
using RuxGymAPI.Repository.Tournament;
using RuxGymAPI.VMModels;
using System.Timers;
using Timer = System.Timers.Timer;

namespace RuxGymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OlimpiaTimeController : ControllerBase
    {

        private readonly ITournament repository;
        private Timer _timer;
        public static bool isTimerElapsed = true;

        public OlimpiaTimeController(ITournament _repository)
        {


            repository = _repository;

        }


        [HttpGet]
        public IActionResult GetTournamentDateAsync()
        {

            string value = repository.TimeUntilNextMondayAtMidnight().ToString();
            

            return Ok(value);
        }
        [HttpGet("StartTimer")]
        public IActionResult StartTimer()
        {
            if(isTimerElapsed)
            {
                repository.StartTime();
            }
           
            return Ok();
        }

        [HttpPut("Admin")]
        public async Task<IActionResult> OlimpiaRewards(string key)
        {
            await repository.OlimpiaReward(key);
            return Ok();
        }

        [HttpGet("Week")]
        public async Task<IActionResult> GetWeek()
        {

            return Ok(await repository.GetOlympiaWeek());
        }


    }
}

