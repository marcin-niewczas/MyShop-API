using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.Commands.ManagementPanel.ProductReviews;
public sealed record RemoveProductReviewMp(
    Guid Id
    ) : ICommand;
