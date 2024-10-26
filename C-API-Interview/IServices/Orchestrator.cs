using Microsoft.AspNetCore.Mvc;

namespace C_API_Interview.IServices;

public interface Orchestrator
{
	public ActionResult StoreFile(Stream stream, StreamWriter streamWriter);
}
