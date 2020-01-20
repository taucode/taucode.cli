(defblock :name checkout :is-top t
	(worker
		:worker-name checkout
		:verb "checkout"
		:doc "Checkout a branch."
		:usage-samples (
			"<todo>"
			"<todo>"
			))

	(idle :name options)

	(opt
		(alt
			(multi-text
				:classes key
				:values "-q" "--quiet"
				:alias quiet
				:action option)	

			(multi-text
				:classes key integer-text
				:values "-2" "--ours"
				:alias ours
				:action option)

			(multi-text
				:classes key integer-text
				:values "-3" "--theirs"
				:alias theirs
				:action option)

			(fallback :name bad-option-fallback)
		)
	)

	(idle :links options next)

	(alt
		(seq
			(multi-text
				:classes key
				:values "-b"
				:alias new-branch
				:action option)
			(some-text
				:classes path
				:alias new-branch-name
				:action argument
			)
			(some-text
				:classes path
				:alias base-branch-name
				:action argument
			)
		)

		(some-text
			:classes path
			:alias existing-branch-name
			:action argument
		)
	)

	(end)
)
