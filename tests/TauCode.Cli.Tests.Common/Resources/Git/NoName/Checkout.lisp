(defblock :name checkout :is-top t
	(worker
		:worker-name checkout
		:verbs "checkout"
		:doc "Checkout a branch."
		:usage-samples (
			"<todo>"
			"<todo>"
			))
	(idle :name args)
	(opt
		(multi-text
			:classes key
			:values "-q" "--quiet"
			:alias quiet
			:action option)
	)
	(opt
		(multi-text
			:classes key integer-text
			:values "-2" "--ours"
			:alias ours
			:action option)
	)
	(opt
		(multi-text
			:classes key integer-text
			:values "-3" "--theirs"
			:alias ours
			:action option)
	)
	(some-text
		:classes path
		:alias branch
		:action argument
	)
	(idle :links args next)
	(end)
)
