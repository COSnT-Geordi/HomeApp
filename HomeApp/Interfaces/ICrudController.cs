using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace HomeApp.Interfaces
{
    public interface ICrudController<T>
    {
        [HttpGet]
        public List<T> GetAll();
        [HttpGet("{id}")]

        public T GetById(int id);
        [HttpPost]
        public T CreateOne([FromBody] T entity);
        [HttpPut]
        public T UpdateOne([FromBody] T entity);
        [HttpDelete("{id}")]
        public bool DeleteOne(int id);

    }
}
