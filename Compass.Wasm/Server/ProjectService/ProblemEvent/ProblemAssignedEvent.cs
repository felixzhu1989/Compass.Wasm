﻿namespace Compass.Wasm.Server.ProjectService.ProblemEvent;

public record ProblemAssignedEvent(string Responder, string Email,string OdoNumber,string ProjectName,string ProblemDesc, DateTime Deadline, string Url);
