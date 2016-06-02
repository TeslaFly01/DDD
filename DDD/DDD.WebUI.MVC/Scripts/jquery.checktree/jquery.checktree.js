(function(jQuery) {
	jQuery.fn.checkTree = function(settings) {
		settings = jQuery.extend({
					onExpand : null,
					onCollapse : null,
					onCheck : null,
					onUnCheck : null,
					onHalfCheck : null,
					onLabelHoverOver : null,
					onLabelHoverOut : null,
					labelAction : "expand",
					debug : false
				}, settings);
		var $tree = this;
		$tree.find("li")//all li
				.find("ul").hide()//all li > ul
				.end() //all li 
				.find(":checkbox").change(function() {//all li > checkbox 
					var $all = jQuery(this).siblings("ul").find(":checkbox");//all checkbox :ul checkbox
					var $checked = $all.filter(":checked");
					if ($all.length == $checked.length && jQuery(this).prop("checked")) {
						jQuery(this).attr("checked", "checked").siblings(".checkbox").removeClass("half_checked").addClass("checked");
						if (settings.onCheck) settings.onCheck(jQuery(this).parent());
					}
					else if ($checked.length == 0) {
						jQuery(this).prop("checked",false).siblings(".checkbox").removeClass("checked").removeClass("half_checked");
						if (settings.onUnCheck)
							settings.onUnCheck(jQuery(this).parent());
					}
					else {
						if (settings.onHalfCheck && !jQuery(this).siblings(".checkbox").hasClass("half_checked"))
							settings.onHalfCheck(jQuery(this).parent());
						jQuery(this).attr("checked", "checked").siblings(".checkbox").removeClass("checked").addClass("half_checked");
					}
				})
				.hide().end()//all li
				.find("label")// all li > lable
				.click(function() {
							var action = settings.labelAction;
							switch (settings.labelAction) {
								case 'expand' :
									jQuery(this).siblings(".arrow").click();
									break;
								case 'check' :
									jQuery(this).siblings(".checkbox").click();
									break;
							}
						})
				.hover(function() { // all li > lable
							jQuery(this).addClass("hover");
							if (settings.onLabelHoverOver)
								settings.onLabelHoverOver(jQuery(this).parent());
						}, function() {
							jQuery(this).removeClass("hover");
							if (settings.onLabelHoverOut)
								settings.onLabelHoverOut(jQuery(this).parent());
						}).end()
				.each(function() { 
					var $arrow = jQuery('<div class="arrow"></div>');
					if (jQuery(this).is(":has(ul)")) { //all li
						$arrow.addClass("collapsed"); // Should start collapsed
						$arrow.click(function() {
									jQuery(this).siblings("ul").slideToggle(0);
									if (jQuery(this).hasClass("collapsed")) {
										// toggled = settings.expandedarrow;
										jQuery(this).addClass("expanded")
												.removeClass("collapsed");
										if (settings.onExpand)
											settings.onExpand(jQuery(this)
													.parent());
									} else {
										// toggled = settings.collapsedarrow;
										jQuery(this).addClass("collapsed")
												.removeClass("expanded");
										if (settings.onCollapse)
											settings.onCollapse(jQuery(this)
													.parent());
									}
								});
					}
					var $checkbox = jQuery('<div class="checkbox"></div>');
					$checkbox.click(function() {
								jQuery(this).removeClass("half_checked").toggleClass("checked");
								if (jQuery(this).hasClass("checked")) { //checkbox image
									if (settings.onCheck)
										settings.onCheck(jQuery(this).parent());
									jQuery(this).siblings(":checkbox").attr("checked", "checked");
									jQuery(this) //checkbox image
											.siblings("ul")//checkbox image >ul
											.find(".checkbox")//checkbox image >ul >all checkbox image
											.not(".checked")
											.removeClass("half_checked")
											.addClass("checked")
											.each(function() {
												if (settings.onCheck)
													settings.onCheck(jQuery(this).parent());
											}).siblings(":checkbox")//checkbox image >ul >all checkbox
											.attr("checked", "checked");
								} else {
									if (settings.onUnCheck)
										settings.onUnCheck(jQuery(this)
												.parent());
									jQuery(this).siblings(":checkbox").prop("checked",false);
									jQuery(this)
											.siblings("ul")
											.find(".checkbox")
											.filter(".checked")
											.removeClass("half_checked")
											.removeClass("checked")
											.each(function() {
												if (settings.onUnCheck)
													settings.onUnCheck(jQuery(this).parent());
											}).siblings(":checkbox")
											.prop("checked",false);
								}
								jQuery(this).parents("ul").siblings(":checkbox").change(); //checkbox image < ul :checkbox
							});
						jQuery(this).prepend($checkbox).prepend($arrow);//all li
				}).end().find(":checkbox").change();
		return $tree;
	};
})(jQuery);