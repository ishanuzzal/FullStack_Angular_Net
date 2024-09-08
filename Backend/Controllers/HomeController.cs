using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Context;
using MyProject.Dtos;
using MyProject.Extension;
using MyProject.Interfaces;
using MyProject.Models;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProduct _product;
        private readonly IAuthentication _auth;
        public HomeController(AppDbContext context,IMapper mapper, IProduct product,IAuthentication auth)
        {
           _context = context;
            _mapper = mapper;
            _product = product;
            _auth = auth;
        }
        [HttpPost]
        [Route("AddProduct")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] ProductCreateDtos productdto)
        {
            var UserName = User.GetUsername();
            var UserId = await _auth.GetUserId(UserName);
            try
            {
                if (ModelState.IsValid)
                {
                    var modelFmDto = _mapper.Map<Product>(productdto);
                    modelFmDto.AppUserId = UserId;
                    var model = await _product.AddProduct(modelFmDto);
                    if (model != null)
                    {
                        return Ok(_mapper.Map<ShowProductDtos>(model));
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return BadRequest();
        }


        [HttpGet]
        [Route("TotalProduct")]
        [Authorize]
        public async Task<IActionResult> TotalProduct()
        {
            var UserName = User.GetUsername();
            var UserId = await _auth.GetUserId(UserName);
            try
            {
                var total = await _product.GetProductCount(UserId);
                return Ok(new { val = total });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("Product/{id}")]
        public async Task<IActionResult> GetProduct([FromRoute]string id)
        {
            try
            {
                var model = await _product.GetProductById(id);
                if (model != null)
                {
                    return Ok(_mapper.Map<ShowProductDtos>(model));
                }
                return NotFound();
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        [Route("AllProduct")]
        [Authorize]
        public async Task<IActionResult> AllProduct([FromQuery] FormQuery form)
        {
            try
            {
                Console.WriteLine($"this is from controller {form.PageNumber} {form.PageSize}");
                var UserName = User.GetUsername();
                var UserId = await _auth.GetUserId(UserName);
                var AllProduct = await _product.GetAllProducts(UserId,form);
                if (AllProduct == null || !AllProduct.Any())
                {
                    return NotFound();
                }
                List<ShowProductDtos> AllshowProduct = new List<ShowProductDtos>();
                foreach (var product in AllProduct)
                {
                    AllshowProduct.Add(_mapper.Map<ShowProductDtos>(product));
                }
                return Ok(AllshowProduct);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        [Route("UpdateProduct/{id}")]
        [Authorize]

        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ProductCreateDtos productdto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var modelFmDto = _mapper.Map<Product>(productdto);
                modelFmDto.ProductId = id;
                var model = await _product.UpdateProduct(modelFmDto);
                if (model != null)
                {
                    return Ok(_mapper.Map<ShowProductDtos>(model));
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        [Authorize]

        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var model = await _product.DeleteProduct(id);
            if (model != null)
            {
                return Ok(_mapper.Map<ShowProductDtos>(model));
            }
            return BadRequest();
        }

    }
}
