using GymProgFramework.LocalStorage;
using GymProgFramework.Models;
using GymProgWebApiBL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GymProgUI.Services
{
    public class ProgramsService : BaseService
    {
        private const String LOCAL_PROGRAMS_FILE_NAME = "programs.json";

        public async Task<ActionResponse> AddNewProgram(ProgramDTO newProgram)
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Post, "programs");
            request.Content = new StringContent(JsonConvert.SerializeObject(newProgram), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ActionResponse() { ErrorMessage = "Operation failed program did not save properly", CompletedSuccessfully = false };

            }

            return JsonConvert.DeserializeObject<ActionResponse>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ActionResponse> UpdateProgram(ProgramDTO newProgram)
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Put, "programs");
            request.Content = new StringContent(JsonConvert.SerializeObject(newProgram), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ActionResponse() { ErrorMessage = "Operation failed program did not save properly", CompletedSuccessfully = false };

            }

            return JsonConvert.DeserializeObject<ActionResponse>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ActionResponse> DeleteProgram(ProgramDTO newProgram)
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Delete, "programs/" + newProgram.Id);

            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ActionResponse() { ErrorMessage = "Operation failed program did not delete properly", CompletedSuccessfully = false };

            }

            return JsonConvert.DeserializeObject<ActionResponse>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ICollection<ProgramDTO>> GetUserPrograms()
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Get, "programs/" + (await new UsersService().GetUser(null)).Name );

            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ICollection<ProgramDTO>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ICollection<ProgramDTO>> GetAllPrograms()
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Get, "programs/");

            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ICollection<ProgramDTO>>(await response.Content.ReadAsStringAsync());
        }

        public ICollection<ProgramDTO> GetLocalPrograms()
        {
            ICollection<ProgramDTO> programs = JsonConvert.DeserializeObject<ICollection<ProgramDTO>>(FileReader.Instance.LoadText(LOCAL_PROGRAMS_FILE_NAME));
            int orderIndex = 0;
            return programs.Select<ProgramDTO,ProgramDTO>(currProgram =>
            {
                currProgram.LocalId = orderIndex;
                orderIndex++;
                return currProgram;
            }).ToList();
        }

        public ActionResponse SetLocalPrograms(ICollection<ProgramDTO> Programs)
        {

            String Serlized = JsonConvert.SerializeObject(Programs);
            FileReader.Instance.SaveText(LOCAL_PROGRAMS_FILE_NAME, Serlized);
            return new ActionResponse() { CompletedSuccessfully = true };
        }


        public ActionResponse SetLocalProgram(ProgramDTO program)
        {
            ICollection<ProgramDTO> localProgram = GetLocalPrograms();

            ProgramDTO[] existingPrograms = localProgram.ToArray();

            existingPrograms[program.LocalId] = program;

            SetLocalPrograms(existingPrograms);

            return new ActionResponse() {CompletedSuccessfully = true };
        }

        public ActionResponse DeleteLocalProgram(ProgramDTO program)
        {
            ICollection<ProgramDTO> localProgram = GetLocalPrograms();

            List<ProgramDTO> existingPrograms = localProgram.ToList();

            existingPrograms.RemoveAt(program.LocalId);

            SetLocalPrograms(existingPrograms);

            return new ActionResponse() { CompletedSuccessfully = true };
        }

    }
}
