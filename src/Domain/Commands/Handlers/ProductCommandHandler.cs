using Domain.Commands.Responses;
using Domain.Contracts.Commands;
using Domain.Contracts.Repositories;
using Domain.Entities;

namespace Domain.Commands.Handlers
{
    public class ProductCommandHandler : Notifiable, IProductCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public GenericResponse Handle(ProductCreateCommand command)
        {
            var exist = _unitOfWork.Products.Exists(command.Name);
            if (exist) 
                return new GenericResponse(false, "Já existe um Produto cadastrado com esse Nome. ", command);                
                
            var product = new Product(command.Name, command.Price);

            AddNotifications(product);
            if (Invalid)
                return new GenericResponse(false, string.Join(". ", Notifications));

            _unitOfWork.Products.Create(product);
            _unitOfWork.Commit();

            return new GenericResponse(true, "Produto cadastrado com sucesso! ", product);
        }
        
        public GenericResponse Handle(ProductUpdateCommand command)
        {
            var product = _unitOfWork.Products.GetById(command.Id);     
            if (product == null) 
                return new GenericResponse(false, "Produto não encontrado na base de dados. ", null);

            var exist = _unitOfWork.Products.ExistsUpdate(command.Name, command.Id);
            if (exist)
                return new GenericResponse(false, "Já existe outro Produto cadatrado com esse Nome. ", null);

            product.Update(command.Name, command.Price);

            AddNotifications(product);
            if (Invalid)
                return new GenericResponse(false, string.Join(". ", Notifications));           

            _unitOfWork.Products.Update(product);
            _unitOfWork.Commit();

            return new GenericResponse(true, "Produto atualizado com sucesso!. ", product);
        }

        public GenericResponse Handle(ProductPromotionCommand command)
        {
            var product = _unitOfWork.Products.GetById(command.Id);     
            if (product == null) 
                return new GenericResponse(false, "Produto não encontrado na base de dados. ", null);           

            product.AddPromotion(command.Price);

            AddNotifications(product);
            if (Invalid)
                return new GenericResponse(false, string.Join(". ", Notifications));           

            _unitOfWork.Products.Update(product);
            _unitOfWork.Commit();

            return new GenericResponse(true, "Preço do Produto atualizado com sucesso! ", product);
        }

        public GenericResponse Handle(ProductDeleteCommand command)
        {
            var product = _unitOfWork.Products.GetById(command.Id);
            if (product == null)
                return new GenericResponse(false, "Produto não encontrado na base de dados. ", null);

            _unitOfWork.Products.Delete(product.Id);
            _unitOfWork.Commit();

            return new GenericResponse(true, "Produco excluído com sucesso! ", product);
        }

    }
}